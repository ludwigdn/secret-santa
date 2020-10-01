using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Interfaces
{
    public interface ImailSender
    {
        public Task Send(List<Participant> participants);
    }
}
