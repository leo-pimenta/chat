using Domain;

namespace Infra.Repositories
{
    internal class WSConnectionInMemoryRepository : IWSConnectionRepository
    {
        private readonly IDictionary<string, string> ConnectionIdByUserId;
        private readonly IDictionary<string, string> UserIdByConnectionId;
        private object OperationLock = new object();

        public WSConnectionInMemoryRepository()
        {
            this.ConnectionIdByUserId = new Dictionary<string, string>();
            this.UserIdByConnectionId = new Dictionary<string, string>();
        }

        public async Task AddAsync(WSConnection wsConnection)
        {
            if (wsConnection == null)
            {
                throw new ArgumentNullException("wsConnection");
            }

            if (string.IsNullOrWhiteSpace(wsConnection.UserId))
            {
                throw new ArgumentException("The connection User Id is invalid: null, empty or just whitespaces.");
            }

            if (string.IsNullOrWhiteSpace(wsConnection.Id))
            {
                throw new ArgumentException("The connection Connection Id is invalid: null, empty or just whitespaces.");
            }

            lock (OperationLock)
            {
                ConnectionIdByUserId.Add(wsConnection.UserId, wsConnection.Id);
                UserIdByConnectionId.Add(wsConnection.Id, wsConnection.UserId);
            }
            
            await Task.Yield();
        }

        public async Task<WSConnection?> GetByConnectionId(string connectionId)
        {
            await Task.Yield();
            return UserIdByConnectionId.ContainsKey(connectionId) 
                ? new WSConnection(connectionId, UserIdByConnectionId[connectionId]) 
                : null;
        }

        public async Task DeleteAsync(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new ArgumentException("Cannot delete by null, empty or whitespaces Connection Id.");
            }

            lock (OperationLock)
            {
                if (UserIdByConnectionId.ContainsKey(connectionId))
                {
                    ConnectionIdByUserId.Remove(UserIdByConnectionId[connectionId]);
                }

                UserIdByConnectionId.Remove(connectionId);
            }
            
            await Task.Yield();
        }
    }
}