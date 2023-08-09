using Logic.Models.EmailConfigurationModel;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfigModel _emailConfig;
        public EmailService(IOptions<EmailConfigModel> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }
        public MimeMessage CreateMessage(Message message)
        {
            var emailMessage = new MimeMessage()
            {
                Subject = message.Subject,
                Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content }
            };
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.EmailFrom));
            emailMessage.To.AddRange(message.EmailTo);

            return emailMessage;
        }

        public async Task SendEmail(Message message)
        {
            var emailMessage = CreateMessage(message);

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
