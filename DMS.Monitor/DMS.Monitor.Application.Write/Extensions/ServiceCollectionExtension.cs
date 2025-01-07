using Microsoft.Extensions.DependencyInjection;

namespace DMS.Monitor.Application.Write.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationWrite(this IServiceCollection services)
    {
        return services;
    }
}
