using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SecretSanta.Models
{
    public class Participant
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<string> GiftIdeas { get; set; } = new List<string>();

        public string ReceiversName => _receiver?.Name;
        public List<string> ReceiversList => _receiver?.GiftIdeas ?? new List<string>();
        public bool IsTaken { get; private set; }
        public bool IsSet => !string.IsNullOrWhiteSpace(ReceiversName) && ReceiversName != Name;
        public bool CanBeTakenBy(Participant santa) => !IsTaken && santa.IsDifferentFrom(this);
        public bool IsDifferentFrom(Participant other) => Name != other.Name && Name != other.ReceiversName;

        private Participant _receiver { get; set; }

        public void SetAsTaken()
        {
            IsTaken = true;
        }

        public void SetReceiver(Participant other)
        {
            _receiver = other;
            other.SetAsTaken();
        }

        public void ExchangeWith(Participant other)
        {
            var temp = other._receiver;
            other.SetReceiver(_receiver);
            SetReceiver(temp);
        }

        public override string ToString()
        {
            return $"{Name} ({Email}) -> {ReceiversName}";
        }
    }
}
