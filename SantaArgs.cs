using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using PowerArgs;

namespace SecretSanta
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    [ArgDescription("Pick your secret santa without needing everyone in the office")]
    public class SantaArgs
    {
        private const int DefaultPort = 465;

        [ArgShortcut("-h"), HelpHook]
        [ArgDescription("Displays the help information")]
        public bool Help { get; set; }

        [ArgRequired(IfNot = "Help"), ArgShortcut("-s")]
        [ArgDescription(
            "The address of the SMTP server with optional port (e.g. smtp.mailtrap.io:465), default port is 465")]
        public string SmtpServer { get; set; } = string.Empty;

        [ArgIgnore]
        public string SmtpServerWithoutPort
        {
            get
            {
                var serverParts = SmtpServer.Split(':', 2, StringSplitOptions.RemoveEmptyEntries);
                return serverParts[0];
            }
        }

        [ArgIgnore]
        public int SmtpPort
        {
            get
            {
                var serverParts = SmtpServer.Split(':', 2, StringSplitOptions.RemoveEmptyEntries);

                if (serverParts.Length != 2)
                {
                    return DefaultPort;
                }

                if (!int.TryParse(serverParts[1], out var port))
                {
                    return DefaultPort;
                }

                return port;
            }
        }

        [ArgRequired(IfNot = "Help"), ArgShortcut("-u")]
        [ArgDescription("User for the SMTP server")]
        public string SmtpUsername { get; set; } = string.Empty;

        [ArgRequired(IfNot = "Help"), ArgShortcut("-p")]
        [ArgDescription("Password for the SMTP server")]
        public string SmtpPassword { get; set; } = string.Empty;

        [ArgRequired(IfNot = "Help"), ArgShortcut("-f")]
        [ArgDescription("The email address of the sender")]
        public string FromAddress { get; set; } = string.Empty;

        [ArgRequired(IfNot = "Help"), ArgShortcut("-i")]
        [ArgDescription("Path to the input CSV file containing participants and their email addresses")]
        [ArgExistingFile]
        public string InputPath { get; set; } = string.Empty;

        [ArgRequired(IfNot = "Help"), ArgShortcut("-m")]
        [ArgDescription("Path to the message HTML file")]
        [ArgExistingFile]
        public string MessagePath { get; set; } = string.Empty;

        public async Task Main()
        {
            Console.WriteLine("  .-\"\"-.");
            Console.WriteLine(" /,..___\\");
            Console.WriteLine("() {_____}");
            Console.WriteLine("  (/-@-@-\\)");
            Console.WriteLine("  {`-=^=-'}");
            Console.WriteLine("  {  `-'  }");
            Console.WriteLine("   {     }");
            Console.WriteLine("    `---'");

            Console.WriteLine("Making list...");
            var selection = PickFromHat();

            try
            {
                var notifier = new Notifier(this);
                Console.WriteLine("Sending list to elves...");
                
                foreach (var (santa, recipient) in selection)
                {
                    await notifier.SendMessageToSanta(santa, recipient);
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"An error occured sending emails: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occured: {ex.Message}");
            }
        }

        private Dictionary<Participant, Participant> PickFromHat()
        {
            var configuration = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true,
            };
            using var csv = new CsvReader(File.OpenText(InputPath), configuration);

            var records = csv.GetRecords<Participant>().ToList();

            var picker = new RandomPicker();
            return picker.GenerateParticipants(records);
        }
    }
}