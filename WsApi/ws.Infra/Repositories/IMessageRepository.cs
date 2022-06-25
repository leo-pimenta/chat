namespace Infra.Repositories
{
    public interface IMessageRepository<TMessage>
    {
        void Add(TMessage message, string key);
    }
}