namespace Domain
{
    public class Message
    {
        public string SenderId { get; set; }
        public string TargetId { get; set; }
        public string Body { get; set; }

        public Message(string senderId, string targetId, string body)
        {
            this.SenderId = senderId;
            this.TargetId = targetId;
            this.Body = body;
        }
    }
}