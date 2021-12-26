using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SecretSanta
{
    public class Notifier
    {
        private const string FromName = "Secret Santa";

        private readonly SmtpClient _client;

        private readonly string _fromEmail;

        private readonly string _messagePath;

        public Notifier(SantaArgs args)
        {
            var server = args.SmtpServerWithoutPort;
            var port = args.SmtpPort;
            var username = args.SmtpUsername;
            var password = args.SmtpPassword;

            _fromEmail = args.FromAddress;
            _messagePath = args.MessagePath;

            _client = new SmtpClient(server, port)
            {
                Credentials = new NetworkCredential(username, password)
            };
        }

        public async Task SendMessageToSanta(Participant santa, Participant recipient)
        {
            var message = new MailMessage(
                new MailAddress(_fromEmail, FromName),
                new MailAddress(santa.Email, santa.Name)
            );

            message.Subject = "Your a secret santa to...";
            message.IsBodyHtml = true;
            message.Body = (await File.ReadAllTextAsync(_messagePath))
                .Replace("{{name}}", recipient.Name);

            await _client.SendMailAsync(message);
        }
    }
}