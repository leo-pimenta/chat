using Infra.Kafka;
using Infra.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Infra.Dependencies
{
    public class InfraDependencyInjector
    {
        public void Inject(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddSingleton<IWSConnectionRepository, WSConnectionInMemoryRepository>();
            InjectKafkaProducer(services, configuration);
        }

        private void InjectKafkaProducer(IServiceCollection services, IConfiguration configuration)
        {
            var clientId = configuration["Kafka:ClientId"];
            var host = Environment.GetEnvironmentVariable("KAFKA_HOST") ?? configuration["KAFKA_HOST"];
            var port = int.Parse(Environment.GetEnvironmentVariable("KAFKA_PORT") ?? configuration["KAFKA_PORT"]);
            var topicName = configuration["Kafka:TopicName"];
            var configurator = new KafkaConfigurator(clientId, host, port);
            var producer = configurator.CreateProducer();
            
            #pragma warning disable CS8604
            services.AddSingleton<IMessageRepository>(services => 
                new KafkaProducer(producer, topicName, services.GetService<IClock>()));
            #pragma warning restore CS8604
        }
    }
}