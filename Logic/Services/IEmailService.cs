using Logic.Models.EmailConfigurationModel;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IEmailService
    {
        public MimeMessage CreateMessage(Message message);
        public Task SendEmail(Message message);
    }
}
