using MassTransit;
using Microsoft.Extensions.Hosting;
using SharedKernel.Contracts;

namespace BoilerClient.Services
{
    public class TemperatureRequestService : IHostedService
    {
        private readonly IRequestClient<TemperatureRequest> _client;

        public TemperatureRequestService(IRequestClient<TemperatureRequest> client)
        {
            _client = client;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                Console.WriteLine("Sending temperature request...");

                var temperature = await _client.GetResponse<TemperatureResponse>(new TemperatureRequest(), cancellationToken);

                Console.WriteLine($"Temperature: {temperature.Message.TemperatureCelsius}°C");

                Thread.Sleep(5000);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
