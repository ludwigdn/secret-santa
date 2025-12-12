using SecretSanta.Interfaces;
using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

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
            Debug.WriteLine(participants);

            do { AssignSantas(participants); }
            while (participants.Any(o => !o.IsSet) || !EnsureEveryonePickedOnlyOnce(participants));

            return participants;
        }

        public static bool EnsureEveryonePickedOnlyOnce(IEnumerable<Participant> participants) {
          var distinctCount = participants.Select(o => o.ReceiversName).Distinct().Count();
          var participantsCount =  participants.Count();
          return distinctCount == participantsCount;
        }

        private void AssignPossibleReceivers(List<Participant> participants)
        {
          foreach(var santa in participants)
          {
            var possibleReceivers = participants.Where(p => p.CanBeTakenBy(santa));
            santa.SetPosibleReceivers(possibleReceivers);
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

                Shuffle(participants);

                var receiver = santa.PossibleReceivers.FirstOrDefault(o => !o.IsTaken);
                if (receiver != null)
                {
                    santa.SetReceiver(receiver);
                    continue;
                }

                receiver = participants.First(o => !o.IsTaken);
                santa.SetReceiver(receiver);
                var tradingPal = participants.First(o => receiver.CanBeTakenBy(o) && santa.HasAsPossibleReceiver(o.ReceiversName));
                tradingPal.ExchangeWith(santa);
            }
        }

        public void Shuffle<Participant>(IList<Participant> participants)
        {
            int amountOfParticipantsToRandomize = participants.Count;
            while (amountOfParticipantsToRandomize > 1)
            {
                amountOfParticipantsToRandomize--;
                int randomIndex = _rand.Next(amountOfParticipantsToRandomize + 1);
                Participant value = participants[randomIndex];
                participants[randomIndex] = participants[amountOfParticipantsToRandomize];
                participants[amountOfParticipantsToRandomize] = value;
            }
        }
    }
}
