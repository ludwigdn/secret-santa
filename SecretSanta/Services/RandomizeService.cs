using SecretSanta.Interfaces;
using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Services
{
    public class RandomizeService : IRandomizer
    {
        Random _rand;

        public RandomizeService()
        {
            _rand = new Random(DateTime.Now.Millisecond);
        }

        public RandomizeService(Random rand)
        {
            _rand = rand;
        }

        public List<Participant> Randomize(List<Participant> participants)
        {
            AssignPossibleReceivers(participants);
            System.Diagnostics.Debug.WriteLine(participants);

            // Assign receivers to santas
            do
            {
              AssignSantas(participants);
            }
            while (participants.Any(o => !o.IsSet) || !EnsureEveryonePickedOnlyOnce(participants));

            // Return the results
            return participants;
        }

        public static bool EnsureEveryonePickedOnlyOnce(IEnumerable<Participant> participants) {
          var distinctCount = participants.Select(o => o.ReceiversName).Distinct().Count();
          var participantsCount =  participants.Count();
          return distinctCount == participantsCount;
        }

        private void AssignPossibleReceivers(List<Participant> participants)
        {
          for (var i = 0; i < participants.Count; i++)
          {
            var santa = participants[i];
            santa.SetPosibleReceivers(participants.Where(p => p.CanBeTakenBy(santa)));
          }
        }

        private void AssignSantas(List<Participant> participants)
        {
            Shuffle(participants);
            var santas = new List<Participant>(participants);
            for (var i = 0; i < santas.Count; i++)
            {
                var santa = santas[0];
                if (santa.IsSet)
                {
                    continue;
                }

                // Shuffle at each iteration to avoid "groups of 3s" (A>B>C, D>E>F, ...)
                Shuffle(participants);

                // Take first available receiver
                var receiver = santa.PossibleReceivers.FirstOrDefault(o => !o.IsTaken);
                if (receiver != null)
                {
                    // Free receiver found: assign them to the santa
                    santa.SetReceiver(receiver);
                    continue;
                }

                // If none found, take the first free receiver, and exchange them with someone that can take them
                receiver = participants.First(o => !o.IsTaken);
                santa.SetReceiver(receiver);
                var tradingPal = participants.First(o => receiver.CanBeTakenBy(o) && santa.HasAsPossibleReceiver(o.ReceiversName));
                tradingPal.ExchangeWith(santa);
            }
        }

        public void Shuffle<Participant>(IList<Participant> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _rand.Next(n + 1);
                Participant value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
