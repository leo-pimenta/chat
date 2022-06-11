using System.Text.Json;
using Confluent.Kafka;
using Domain;

namespace Infra.Kafka
{
    public class MessageSerializer : ISerializer<Message>
    {
        public byte[] Serialize(Message data, SerializationContext context) => 
            JsonSerializer.SerializeToUtf8Bytes(data);
    }
}