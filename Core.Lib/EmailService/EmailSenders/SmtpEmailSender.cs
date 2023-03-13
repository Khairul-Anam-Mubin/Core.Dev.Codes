using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Lib.EmailService.Interfaces;
using Core.Lib.EmailService.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Core.Lib.EmailService.EmailSenders
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfiguration;
        
        public SmtpEmailSender(IConfiguration configuration)
        {
            _emailConfiguration = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        }
        
        public async Task<bool> SendEmailAsync(Message message)
        {
            var emailMessage = CreateMimeMessage(message);
            try
            {
                await SendAsync(emailMessage);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private MimeMessage CreateMimeMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfiguration.From));
            var to = new List<MailboxAddress>();
            to.AddRange(message.To.Select(x => new MailboxAddress("email", x)));
            emailMessage.To.AddRange(to);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = message.Content
            };
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage message)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailConfiguration.Server, _emailConfiguration.Port);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);
                await client.SendAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
