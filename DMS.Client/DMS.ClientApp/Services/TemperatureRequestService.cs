using DMS.Monitor.Contracts.Public.Events.BoilerDevice;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace DMS.ClientApp.Services;

public class TemperatureRequestService : IHostedService
{
    private readonly IBus _bus;

    public TemperatureRequestService(IBus bus)
    {
        _bus = bus;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("Sending temperature request...");

            try
            {
                await _bus.Publish(new BoilerTemperatureRequestedEvent(), cancellationToken);
                Console.WriteLine("Temperature request sent");
            }
            catch (RequestTimeoutException)
            {
                Console.WriteLine("Connection failed due to timeout");
            }

            await Task.Delay(5000, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
