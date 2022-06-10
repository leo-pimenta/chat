namespace Domain
{
    public class WSConnection
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        public WSConnection(string connectionId, string userId)
        {
            this.Id = connectionId;
            this.UserId = userId;
        }
    }
}