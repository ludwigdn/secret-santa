using SecretSanta.Interfaces;
using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
                var receiver = participants.FirstOrDefault(o => o.CanBeTakenBy(santa));
                if (receiver != null)
                {
                    // Free receiver found: assign them to the santa
                    santa.SetReceiver(receiver);
                    continue;
                }

                // If none found, take the first free receiver
                receiver = participants.FirstOrDefault(o => !o.IsTaken);
                
                santa.SetReceiver(receiver);

                // Get first assigned santa, that differs from current and exchange their receivers
                participants.Where(o => o.IsTaken)
                    .Where(o => o.ReceiverName != santa.Name)
                    .First(o => o.Name != receiver.Name)
                    .ExchangeWith(santa);
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
