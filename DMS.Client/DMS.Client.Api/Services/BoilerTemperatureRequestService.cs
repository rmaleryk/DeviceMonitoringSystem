using DMS.Monitor.Contracts.Public.Events.BoilerDevice;
using MassTransit;

namespace DMS.Client.Api.Services;

public class BoilerTemperatureRequestService(
    IBus bus,
    ILogger<BoilerTemperatureRequestService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Sending temperature request...");

            try
            {
                await bus.Publish(new BoilerTemperatureRequestedEvent(), stoppingToken);
                logger.LogInformation("Temperature request sent");
            }
            catch (RequestTimeoutException ex)
            {
                logger.LogError(ex, "Connection failed due to timeout");
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}
