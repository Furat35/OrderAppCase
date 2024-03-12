using Microsoft.Extensions.Configuration;
using Ordering.Application.Mail;
using System.Net;
using System.Net.Mail;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public EmailService(IConfiguration configuration)
        {
            _smtpServer = configuration["MailConfigurations:SmtpServer"];
            _smtpPort = int.Parse(configuration["MailConfigurations:SmtpPort"]);
            _smtpUsername = configuration["MailConfigurations:SmtpUsername"];
            _smtpPassword = configuration["MailConfigurations:SmtpPassword"];
        }

        public async Task SendEmailAsync(string toAddress, string subject, string body, bool isHtml = false)
        {
            try
            {
                using (var client = new SmtpClient(_smtpServer))
                {
                    client.Port = _smtpPort;
                    client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);

                    var message = new MailMessage
                    {
                        From = new MailAddress(_smtpUsername),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = isHtml
                    };

                    message.To.Add(toAddress);

                    await client.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
