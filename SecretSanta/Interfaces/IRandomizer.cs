using SecretSanta.Models;
using System.Collections.Generic;

namespace SecretSanta.Interfaces
{
    public interface IRandomizer
    {
        public List<Participant> Randomize(List<Participant> participants);
    }
}
