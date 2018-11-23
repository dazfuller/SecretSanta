using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace SecretSanta
{
    public class Notifier
    {
        private readonly IConfigurationRoot _config;
        
        private readonly SmtpClient _client;

        private readonly string _fromName;

        private readonly string _fromEmail;

        public Notifier()
        {
            _config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", false)
               .Build();

            var server = _config["Smtp:Server"];
            var port = Convert.ToInt32(_config["Smtp:Port"]);
            var username = _config["Smtp:Username"];
            var password = _config["Smtp:Password"];

            _fromName = _config["Smtp:FromName"];
            _fromEmail = _config["Smtp:FromEmail"];

            _client = new SmtpClient(server, port)
            {
                Credentials = new NetworkCredential(username, password)
            };
        }

        public async Task SendMessageToSanta(Participant santa, Participant recipient)
        {
            var message = new MailMessage(
                new MailAddress(_fromEmail, _fromName),
                new MailAddress(santa.Email, santa.Name)
            );

            message.Subject = "Your a secret santa to...";
            message.IsBodyHtml = true;
            message.Body = File.ReadAllText(_config["MessagePath"])
                .Replace("{{name}}", recipient.Name);
            
            await _client.SendMailAsync(message);
        }
    }
}