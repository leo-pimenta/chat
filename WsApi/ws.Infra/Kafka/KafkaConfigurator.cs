using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Domain;

namespace Infra.Kafka
{
    internal class KafkaConfigurator
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

        public IProducer<string, Message> CreateProducer() => new ProducerBuilder<string, Message>(Config)
                .SetValueSerializer(new MessageSerializer())
                .Build();
    }
}