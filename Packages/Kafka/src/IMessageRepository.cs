namespace Repositories.Messages
{
    public interface IMessageRepository<TMessage>
    {
        void Add(TMessage message, string key);
    }
}