using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Dependencies
{
    public static class InfraDependencyInjector
    {
        public static void Inject(IServiceCollection services)
        {
            services.AddSingleton<IWSConnectionRepository, WSConnectionInMemoryRepository>();
        }
    }
}