using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using CsvHelper;
using PowerArgs;

namespace SecretSanta
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    [ArgDescription("Pick your secret santas without needing everyone in the office")]
    public class SantaArgs
    {
        private const int DefaultPort = 465;

        [ArgShortcut("-h"), HelpHook]
        [ArgDescription("Displays the help information")]
        public bool Help { get; set; }

        [ArgRequired(IfNot = "Help"), ArgShortcut("-s")]
        [ArgDescription("The address of the SMTP server with optional port (e.g. smtp.mailtrap.io:465), default port is 465")]
        public string SmtpServer { get; set; }

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
        public string SmtpUsername { get; set; }

        [ArgRequired(IfNot = "Help"), ArgShortcut("-p")]
        [ArgDescription("Password for the SMTP server")]
        public string SmtpPassword { get; set; }

        [ArgRequired(IfNot = "Help"), ArgShortcut("-f")]
        [ArgDescription("The email address of the sender")]
        public string FromAddress { get; set; }

        [ArgRequired(IfNot = "Help"), ArgShortcut("-i")]
        [ArgDescription("Path to the input CSV file containing participants and their email addresses")]
        [ArgExistingFile]
        public string InputPath { get; set; }

        [ArgRequired(IfNot = "Help"), ArgShortcut("-m")]
        [ArgDescription("Path to the message HTML file")]
        [ArgExistingFile]
        public string MessagePath { get; set; }

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

            Console.WriteLine("Checking list...");
            Console.WriteLine("Checking List...");

            try
            {
                var notifier = new Notifier(this);
                Console.WriteLine("Sending list to elves...");
                foreach (var pair in selection)
                {
                    await notifier.SendMessageToSanta(pair.Key, pair.Value);
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"An error occured sending emails: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occured: {ex.Message}");
                return;
            }
        }

        private Dictionary<Participant, Participant> PickFromHat()
        {
            using (var reader = new StreamReader(File.OpenRead(InputPath)))
            {
                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.HasHeaderRecord = true;
                    var records = csv.GetRecords<Participant>().ToList();
                    
                    var picker = new RandomPicker();
                    return picker.GenerateParticipants(records);
                }
            }
        }
    }
}
