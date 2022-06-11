using Domain;

namespace Infra.Repositories
{
    public interface IMessageRepository
    {
        void Add(Message message);
    }
}