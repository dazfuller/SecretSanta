using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using PowerArgs;

namespace SecretSanta
{
    public class SantaArgs
    {
        [ArgRequired]
        [ArgShortcut("-i")]
        [ArgDescription("Path to the input CSV file containing participants and their email addresses")]
        [ArgExistingFile]
        public string InputPath { get; set; }

        public async Task Main()
        {
            var selection = PickFromHat();
            var notifier = new Notifier();
            foreach (var pair in selection)
            {
                await notifier.SendMessageToSanta(pair.Key, pair.Value);
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
