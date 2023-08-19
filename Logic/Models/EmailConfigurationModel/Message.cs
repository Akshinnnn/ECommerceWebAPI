using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models.EmailConfigurationModel
{
    public class Message
    {
        public Message(List<string> emailTo, string subject, string content)
        {
            EmailTo = new List<MailboxAddress>();
            EmailTo.AddRange(emailTo.Select(x => new MailboxAddress("email", x)));
            Subject = subject;
            Content = content;
        }

        public List<MailboxAddress> EmailTo{ get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
