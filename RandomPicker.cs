using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta
{
    public class RandomPicker
    {
        private Random _random;

        public RandomPicker()
        {
            _random = new Random();
        }

        public Dictionary<Participant, Participant> GenerateParticipants(IEnumerable<Participant> participants)
        {
            var isValid = true;
            var finalRecipients = new Dictionary<Participant, Participant>();

            do
            {
                isValid = true;
                var presentReceipients = new List<Participant>();
                finalRecipients = new Dictionary<Participant, Participant>();

                foreach (var person in participants)
                {
                    var hat = participants
                        .Where(p => p.Name != person.Name)
                        .Where(p => !presentReceipients.Contains(p))
                        .ToList();

                    if (!hat.Any())
                    {
                        isValid = false;
                        break;
                    }

                    hat.Shuffle(_random);

                    var recipient = hat.First();

                    presentReceipients.Add(recipient);
                    finalRecipients.Add(person, recipient);
                }
            } while (!isValid);

            return finalRecipients;
        }
    }
}