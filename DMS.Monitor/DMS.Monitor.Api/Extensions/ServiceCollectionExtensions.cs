namespace DMS.Monitor.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddHealthChecks();

        return services;
    }
}