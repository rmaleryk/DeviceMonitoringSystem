using Microsoft.Extensions.Caching.Memory;

namespace DMS.Client.Api.Caching;

public class BoilerTemperatureCache(
    IMemoryCache memoryCache,
    ILogger<BoilerTemperatureCache> logger)
{
    private const string CacheKey = "boiler_temperature";

    public double? GetTemperature()
    {
        var hasTemperature = memoryCache.TryGetValue<double>(CacheKey, out var temperature);
        if (!hasTemperature)
        {
            logger.LogWarning("Cache doesn't contain boiler temperature.");
            return null;
        }

        return temperature;
    }

    public void SetTemperature(double temperature)
    {
        memoryCache.Set(CacheKey, temperature);
    }
}
