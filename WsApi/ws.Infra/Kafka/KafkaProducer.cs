using Confluent.Kafka;
using Domain;
using Infra.Repositories;
using NodaTime;

namespace Infra.Kafka
{
    internal class KafkaProducer : IMessageRepository
    {
        private readonly IProducer<string, Message> Producer;
        private readonly string TopicName;
        private readonly IClock Clock;

        public KafkaProducer(IProducer<string, Message> producer, string topicName, IClock clock)
        {
            producer.ThrowIfNull("producer");
            topicName.ThrowIfNullOrWhitespace("Invalid topic name.");
            clock.ThrowIfNull("clock");
            this.Producer = producer;
            this.TopicName = topicName;
            this.Clock = clock;
        }

        public void Add(Message message)
        {
            ThrowIfInvalidMessage(message);
            var kafkaMessage = new Confluent.Kafka.Message<string, Message>()
            {
                Key = message.TargetId,
                Timestamp = GetCurrentTimestamp(),
                Value = message
            };

            Producer.Produce(TopicName, kafkaMessage);
        }

        private void ThrowIfInvalidMessage(Message message)
        {
            message.ThrowIfNull("message");
            if (!message.IsValid())
            {
                throw new ArgumentException("Invalid message.");
            }
        }

        private Timestamp GetCurrentTimestamp() =>
            new Timestamp(
                Clock.GetCurrentInstant().ToUnixTimeMilliseconds(),
                TimestampType.CreateTime);
    }
}