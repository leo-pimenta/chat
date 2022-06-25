using Confluent.Kafka;

namespace Kafka
{
    public class KafkaConfigurator<TMessage, TMessageSerializer>
        where TMessageSerializer : ISerializer<TMessage>, new()
    {
        private readonly ClientConfig Config;

        public KafkaConfigurator(string clientId, string kafkaHost, int kafkaHostPort)
        {
            this.Config = new ClientConfig()
            {
                BootstrapServers = $"{kafkaHost}:{kafkaHostPort}",
                ClientId = clientId,
                Acks = Acks.Leader,
            };
        }

        public IProducer<string, TMessage> CreateProducer() => new ProducerBuilder<string, TMessage>(Config)
                .SetValueSerializer(new TMessageSerializer())
                .Build();
    }
}