using Confluent.Kafka;
using Infra.Repositories;
using NodaTime;

namespace Infra.Kafka
{
    internal class KafkaProducer<TMessage> : IMessageRepository<TMessage>
    {
        private readonly IProducer<string, TMessage> Producer;
        private readonly string TopicName;
        private readonly IClock Clock;

        public KafkaProducer(IProducer<string, TMessage> producer, string topicName, IClock clock)
        {
            producer.ThrowIfNull("producer");
            topicName.ThrowIfNullOrWhitespace("Invalid topic name.");
            clock.ThrowIfNull("clock");
            this.Producer = producer;
            this.TopicName = topicName;
            this.Clock = clock;
        }

        public void Add(TMessage message, string key)
        {
            message.ThrowIfNull("message");
            key.ThrowIfNullOrWhitespace("Invalid key.");
            var kafkaMessage = new Confluent.Kafka.Message<string, TMessage>()
            {
                Key = key,
                Timestamp = GetCurrentTimestamp(),
                Value = message
            };

            Producer.Produce(TopicName, kafkaMessage);
        }

        private Timestamp GetCurrentTimestamp() =>
            new Timestamp(
                Clock.GetCurrentInstant().ToUnixTimeMilliseconds(),
                TimestampType.CreateTime);
    }
}