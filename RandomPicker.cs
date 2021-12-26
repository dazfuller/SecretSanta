using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta
{
    public class RandomPicker
    {
        private readonly Random _random;

        public RandomPicker()
        {
            _random = new Random();
        }

        public Dictionary<Participant, Participant> GenerateParticipants(IList<Participant> participants)
        {
            bool isValid;
            Dictionary<Participant, Participant> finalRecipients;

            do
            {
                Console.WriteLine("Checking list...");
                
                isValid = true;
                var presentRecipients = new List<Participant>();
                finalRecipients = new Dictionary<Participant, Participant>();

                foreach (var person in participants)
                {
                    var hat = participants
                        .Where(p => p.Name != person.Name)
                        .Where(p => !presentRecipients.Contains(p))
                        .ToList();

                    if (!hat.Any())
                    {
                        isValid = false;
                        break;
                    }

                    hat.Shuffle(_random);

                    var recipient = hat.First();

                    presentRecipients.Add(recipient);
                    finalRecipients.Add(person, recipient);
                }
            } while (!isValid);

            return finalRecipients;
        }
    }
}