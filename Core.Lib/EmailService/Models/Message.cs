using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MimeKit;

namespace Core.Lib.EmailService.Models
{
    public class Message
    {
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool IsHtmlContent { get; set; }
        public IFormFileCollection Attachments { get; set; }
    }
}
