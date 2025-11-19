using System.Collections.Generic;
using System.Threading.Tasks;
using Garage.Domain.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Garage.Domain.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlBody, List<string> attachments = null)
        {
            var fromName = _configuration["EmailSettings:FromName"];
            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var host = _configuration["EmailSettings:Host"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var useSSL = bool.Parse(_configuration["EmailSettings:UseSSL"]);
            var userName = _configuration["EmailSettings:UserName"];
            var password = _configuration["EmailSettings:Password"];

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(fromName, fromEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };

            if (attachments != null)
            {
                foreach (var file in attachments)
                {
                    builder.Attachments.Add(file);
                }
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                host,
                port,
                useSSL ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(userName, password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
