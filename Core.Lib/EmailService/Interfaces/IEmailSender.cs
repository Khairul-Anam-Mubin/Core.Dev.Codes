using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Lib.EmailService.Models;

namespace Core.Lib.EmailService.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(Message message);
    }
}
