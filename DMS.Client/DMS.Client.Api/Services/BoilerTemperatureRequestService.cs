using DMS.Client.Api.Configuration;
using DMS.Monitor.Contracts.Public.Events.Boilers;
using MassTransit;
using Microsoft.Extensions.Options;

namespace DMS.Client.Api.Services;

public class BoilerTemperatureRequestService(
    IBus bus,
    IOptions<BoilerConfiguration> boilerConfiguration,
    ILogger<BoilerTemperatureRequestService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Sending temperature request...");

            try
            {
                var boilerId = boilerConfiguration.Value.Id!.Value;
                await bus.Publish(new BoilerTemperatureRequestedIntegrationEvent(boilerId), stoppingToken);
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
