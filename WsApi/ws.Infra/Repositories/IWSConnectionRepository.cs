using Domain;

namespace Infra.Repositories
{
    public interface IWSConnectionRepository
    {
        Task AddAsync(WSConnection wsConnection);
        Task DeleteAsync(string connectionId);
        Task<WSConnection?> GetByConnectionId(string connectionId);
    }
}