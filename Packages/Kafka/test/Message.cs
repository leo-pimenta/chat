namespace Test
{
    public class Message
    {
        public string Key { get; set; }
        public string Body { get; set; }

        public Message(string key, string body)
        {
            Key = key;
            Body = body;
        }
    }
}