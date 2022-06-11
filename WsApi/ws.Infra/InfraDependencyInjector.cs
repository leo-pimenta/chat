using Infra.Kafka;
using Infra.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Infra.Dependencies
{
    public class InfraDependencyInjector
    {
        public async Task InjectAsync(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddSingleton<IWSConnectionRepository, WSConnectionInMemoryRepository>();
            await InjectKafkaProducerAsync(services, configuration);
        }

        private async Task InjectKafkaProducerAsync(IServiceCollection services, IConfiguration configuration)
        {
            var clientId = configuration["Kafka:ClientId"];
            var host = configuration["Kafka:Host"];
            var port = int.Parse(configuration["Kafka:Port"]);
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