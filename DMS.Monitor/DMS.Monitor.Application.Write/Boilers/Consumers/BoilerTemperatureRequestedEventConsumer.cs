using DMS.Monitor.Application.Write.Boilers.Commands;
using DMS.Monitor.Contracts.Public.Events.Boilers;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DMS.Monitor.Application.Write.Boilers.Consumers;

public class BoilerTemperatureRequestedEventConsumer(
    ISender sender,
    ILogger<BoilerTemperatureRequestedEventConsumer> logger) : IConsumer<BoilerTemperatureRequestedEvent>
{
    public async Task Consume(ConsumeContext<BoilerTemperatureRequestedEvent> context)
    {
        logger.LogInformation("Requested updating temperature for the boiler with id {Id}", context.Message.Id);
        await sender.Send(new UpdateBoilerTemperatureCommand(context.Message.Id), context.CancellationToken);
    }
}
