using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Interfaces
{
    public interface IRandomizer
    {
        public List<Participant> Randomize(List<Participant> participants);
    }
}
