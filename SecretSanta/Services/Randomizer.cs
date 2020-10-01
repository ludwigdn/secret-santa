using SecretSanta.Interfaces;
using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta
{
    public class Randomizer : IRandomizer
    {
        private Random rand;

        public Randomizer()
        {
            rand = new Random(DateTime.Now.Millisecond);
        }

        public List<Participant> Randomize(List<Participant> participants)
        {
            // Assign receivers to santas
            do { AssignSantas(participants); }
            while (participants.Any(o => !o.IsSet));

            // Return the results
            return participants;
        }

        private void AssignSantas(List<Participant> participants)
        {
            Shuffle(participants);
            var santas = new List<Participant>(participants);
            for (var i = 0; i < santas.Count; i++)
            {
                var santa = santas[0];
                if (santa.IsSet)
                    continue;

                // Shuffle at each iteration to avoid "groups of 3s" (A>B>C, D>F>G, ...)
                Shuffle(participants);

                // Take first free participant that differs from the current Santa
                var receiver = participants.FirstOrDefault(o => !o.IsTaken(participants) && santa.IsDifferent(o));
                if (receiver != null)
                {
                    // Free receiver found: assign them to the santa
                    santa.SetReceiver(receiver.Name);
                    continue;
                }

                // If none found, take the first free receiver
                receiver = participants.FirstOrDefault(o => !o.IsTaken(participants));
                santa.SetReceiver(receiver.Name);

                // Get first assigned santa, that differs from current ande xchange their receivers
                participants.Where(o => o.IsTaken(participants))
                    .Where(o => o.ReceiverName != santa.Name)
                    .First(o => o.Name != receiver.Name)
                    .ExchangeWith(santa);       
            }
        }

        private void Shuffle<Participant>(IList<Participant> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                Participant value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
