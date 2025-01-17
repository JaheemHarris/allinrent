using System.Net.Mail;
using System.Net;
using MvcAllinRent.Models;
using MvcAllinRent.Interfaces;
using Microsoft.Extensions.Options;

namespace MvcAllinRent.Utils
{
    public class EmailService: IEmailService
    {
        private readonly EmailConfigs _emailConfigs;

        public EmailService(IOptions<EmailConfigs> emailConfigs)
        {
            _emailConfigs = emailConfigs.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_emailConfigs.SmtpHost, _emailConfigs.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailConfigs.SmtpEmail, _emailConfigs.SmtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailConfigs.SmtpEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
        }
    }
}
