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


        public string ReceiverName { get; private set; }
        public bool IsTaken { get; private set; }

        public bool IsSet => !string.IsNullOrWhiteSpace(ReceiverName) && ReceiverName != Name;
        public bool CanBeTakenBy(Participant santa) => !IsTaken && santa.IsDifferentFrom(this);
        public bool IsDifferentFrom(Participant other) => Name != other.Name && Name != other.ReceiverName;

        public void SetAsTaken()
        {
            IsTaken = true;
        }

        public void SetReceiver(Participant other)
        {
            ReceiverName = other.Name;
            other.SetAsTaken();
        }

        public void ExchangeWith(Participant other)
        {
            var temp = other.ReceiverName;
            other.SetReceiver(this);
            ReceiverName = temp;
            other.SetAsTaken();
        }

        public bool HasGiftIdeas()
        {
            return GiftIdeas?.Any(o => !string.IsNullOrWhiteSpace(o)) ?? false;
        }

        public override string ToString()
        {
            return $"{Email} - {Name} -> {ReceiverName}";
        }
    }
}
