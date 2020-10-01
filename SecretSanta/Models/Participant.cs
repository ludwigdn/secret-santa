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

        public string ReceiverName { get; private set; }

        public bool IsSet => !string.IsNullOrWhiteSpace(ReceiverName) && ReceiverName != Name;

        public bool IsTaken(IEnumerable<Participant> Participants) => Participants.Any(o => o.ReceiverName == Name);

        public void SetReceiver(string name)
        {
            ReceiverName = name;
        }

        public void ExchangeWith(Participant otherSanta)
        {
            var temp = ReceiverName;
            SetReceiver(otherSanta.ReceiverName);
            otherSanta.SetReceiver(temp);
        }

        public bool IsDifferent(Participant receiver)
        {
            return Name != receiver.Name && Name != receiver.ReceiverName;
        }

        public override string ToString()
        {
            return $"{Email} - {Name} -> {ReceiverName}";
        }
    }
}
