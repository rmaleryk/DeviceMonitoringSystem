using Microsoft.Extensions.DependencyInjection;

namespace DMS.Monitor.Application.Read.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationRead(this IServiceCollection services)
    {
        return services;
    }
}
