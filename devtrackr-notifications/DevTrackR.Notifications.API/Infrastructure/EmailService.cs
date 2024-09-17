using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace DevTrackR.Notifications.API.Infrastructure
{
    public class EmailService : INotificationService
    {
        private readonly string? _host;
        private readonly int _port;
        private readonly string? _username;
        private readonly string? _password;
        private readonly string? _from;
        private readonly string? _name;

        public EmailService(IConfiguration configuration)
        {
            _host = configuration["Smtp:Host"];
            _port = int.Parse(configuration["Smtp:Port"]);
            _username = configuration["Smtp:Username"];
            _password = configuration["Smtp:Password"];
            _from = configuration["Smtp:From"];
            _name = configuration["Smtp:Name"];
        }

        public async Task Send(IEmailTemplate template)
        {
            using (var client = new SmtpClient(_host, _port))
            {
                client.Credentials = new NetworkCredential(_username, _password);
                client.EnableSsl = true;

                var message = new MailMessage
                {
                    From = new MailAddress(_from, _name),
                    Subject = template.Subject,
                    Body = template.Content,
                    IsBodyHtml = true
                };

                message.To.Add(template.To);

                await client.SendMailAsync(message);
            }
        }
    }
}
