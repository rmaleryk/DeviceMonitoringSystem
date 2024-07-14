using BoilerMonitor.Domain.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace BoilerMonitor.Domain.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDomainConverters(this IServiceCollection services)
        {
            services.AddTransient<ITemperatureConverter, TemperatureConverter>();

            return services;
        }
    }
}
