using AspNetTemplate.ClientEntity;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetTemplate.CommonService
{
    public class EmailService : IEmailService
    {
        private readonly AppSettings _appSettings;

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string messageStr)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_appSettings.MailSettings.From));
            message.To.Add(new MailboxAddress(to));
            message.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = messageStr;
            message.Body = bodyBuilder.ToMessageBody();


            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_appSettings.MailSettings.Host, int.Parse(_appSettings.MailSettings.Port));

                client.Authenticate(_appSettings.MailSettings.Username, _appSettings.MailSettings.Password);

                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
