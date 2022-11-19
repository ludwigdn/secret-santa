using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SecretSanta.Models
{
    public class Participant
    {
        // Properties
        // ==========

        // Private
        private Participant _receiver { get; set; }

        // Public
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<string> GiftIdeas { get; set; } = new List<string>();

        [DataMember]
        public string Partner { get; set; }

        public List<Participant> PossibleReceivers { get; set; }

        public List<string> PossibleReceiversNames { get; set; }

        public bool IsTaken { get; private set; }

        public string ReceiversName => _receiver?.Name;

        public List<string> ReceiversList => _receiver.GiftIdeas;


        // Functions
        // =========

        public bool IsSet =>
          !string.IsNullOrWhiteSpace(ReceiversName);

        public bool HasAsPossibleReceiver(string possibleReceiversName) =>
          PossibleReceiversNames.Contains(possibleReceiversName);

        public bool CanBeTakenBy(Participant santa) =>
          Name != santa.Name &&
          Name != santa.Partner;

        public void SetPosibleReceivers(IEnumerable<Participant> possibleReceivers)
        {
          PossibleReceivers = possibleReceivers.ToList();
          PossibleReceiversNames = possibleReceivers.Select(o => o.Name).ToList();
        }

        public void SetAsTaken()
        {
          IsTaken = true;
        }

        public void SetReceiver(Participant receiver)
        {
            _receiver = receiver;
            _receiver.SetAsTaken();
        }

        public void ExchangeWith(Participant tradingPal)
        {
            var new_receiver = tradingPal._receiver;
            tradingPal.SetReceiver(_receiver);
            SetReceiver(new_receiver);
        }

        public override string ToString() => $"{Name} ({Email}) " +
          $"| Partner is {Partner ?? "nobody"} " +
          $"| Receiver is {ReceiversName}";
    }
}
