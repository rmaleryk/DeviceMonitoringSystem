using Microsoft.Extensions.Caching.Memory;

namespace DMS.Client.Api.Caching;

public class BoilerTemperatureCache(
    IMemoryCache memoryCache,
    ILogger<BoilerTemperatureCache> logger)
{
    public double? GetTemperature(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        var hasTemperature = memoryCache.TryGetValue<double>(cacheKey, out var temperature);
        if (!hasTemperature)
        {
            logger.LogWarning("Cache doesn't contain boiler temperature.");
            return null;
        }

        return temperature;
    }

    public void SetTemperature(Guid id, double temperature)
    {
        var cacheKey = GetCacheKey(id);
        memoryCache.Set(cacheKey, temperature);
    }

    private static string GetCacheKey(Guid id) => $"boiler_{id}_temperature";
}
