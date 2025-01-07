using DMS.Monitor.Application.Write.Configuration;
using DMS.Monitor.Domain.Boilers.Enums;
using DMS.Monitor.Domain.Converters;
using DMS.Monitor.Domain.Persistence;
using DMS.Monitor.Infrastructure.BoilerApi.Client;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DMS.Monitor.Application.Write.Boilers.Commands;

public class BoilerCommandHandler :
    IRequestHandler<TurnOnBoilerCommand, CommandResult>,
    IRequestHandler<TurnOffBoilerCommand, CommandResult>,
    IRequestHandler<UpdateBoilerTemperatureCommand, CommandResult>
{
    private readonly IBoilerRepository _boilerRepository;
    private readonly IBoilerApiClient _boilerApiClient;
    private readonly ITemperatureConverter _temperatureConverter;
    private readonly BoilerTemperatureThresholds _boilerTemperatureThresholds;
    private readonly ILogger<BoilerCommandHandler> _logger;

    public BoilerCommandHandler(
        IBoilerRepository boilerRepository,
        IBoilerApiClient boilerApiClient,
        ITemperatureConverter temperatureConverter,
        IOptions<BoilerTemperatureThresholds> boilerTemperatureThresholds,
        ILogger<BoilerCommandHandler> logger)
    {
        _boilerRepository = boilerRepository;
        _boilerApiClient = boilerApiClient;
        _temperatureConverter = temperatureConverter;
        _boilerTemperatureThresholds = boilerTemperatureThresholds.Value;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(TurnOnBoilerCommand command, CancellationToken cancellationToken)
    {
        var boiler = await _boilerRepository.GetByIdAsync(command.Id);
        if (boiler is null)
        {
            return CommandResult.Failed(Guid.Empty, $"Boiler with id {command.Id} does not exist.");
        }

        if (boiler.State is BoilerState.On)
        {
            return CommandResult.Conflict(boiler.Id, $"Boiler with id {command.Id} is already turned on.");
        }

        boiler.TurnOn();
        await _boilerRepository.SaveChangesAsync(cancellationToken);

        return CommandResult.Completed(boiler.Id);
    }

    public async Task<CommandResult> Handle(TurnOffBoilerCommand command, CancellationToken cancellationToken)
    {
        var boiler = await _boilerRepository.GetByIdAsync(command.Id);
        if (boiler is null)
        {
            return CommandResult.Failed(Guid.Empty, $"Boiler with id {command.Id} does not exist.");
        }

        if (boiler.State is BoilerState.Off)
        {
            return CommandResult.Conflict(boiler.Id, $"Boiler with id {command.Id} is already turned off.");
        }

        boiler.TurnOff();
        await _boilerRepository.SaveChangesAsync(cancellationToken);

        return CommandResult.Completed(boiler.Id);
    }

    public async Task<CommandResult> Handle(UpdateBoilerTemperatureCommand command, CancellationToken cancellationToken)
    {
        var boiler = await _boilerRepository.GetByIdAsync(command.Id);
        if (boiler is null)
        {
            return CommandResult.Failed(Guid.Empty, $"Boiler with id {command.Id} does not exist.");
        }

        var newTemperatureFahrenheit = await _boilerApiClient.GetTemperatureFahrenheitAsync(command.Id, cancellationToken);
        var newTemperatureCelsius = _temperatureConverter.ToCelsius(newTemperatureFahrenheit);

        var isValidTemperature = boiler.UpdateTemperature(
            newTemperatureCelsius,
            _boilerTemperatureThresholds.MinTemperature!.Value,
            _boilerTemperatureThresholds.MaxTemperature!.Value);

        if (!isValidTemperature)
        {
            _logger.LogError(
                "Boiler with id {Id} has temperature out of range: {Temperature}°C",
                command.Id,
                newTemperatureCelsius);
        }

        await _boilerRepository.SaveChangesAsync(cancellationToken);

        return CommandResult.Completed(boiler.Id);
    }
}
