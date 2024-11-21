using SecretSanta.Models;
using System.Threading.Tasks;

namespace SecretSanta.Interfaces
{
    public interface ImailSender
    {
        public Task Send(Participant santa, string body, string subject);
    }
}
