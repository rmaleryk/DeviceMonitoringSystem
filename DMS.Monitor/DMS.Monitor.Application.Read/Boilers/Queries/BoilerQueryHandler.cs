using DMS.Monitor.Domain.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DMS.Monitor.Application.Read.Boilers.Queries;

public sealed class BoilerQueryHandler :
    IRequestHandler<GetBoilersQuery, QueryResult<IReadOnlyCollection<GetBoilersResult>>>,
    IRequestHandler<GetBoilerQuery, QueryResult<GetBoilerResult>>
{
    private readonly IBoilerRepository _boilerRepository;
    private readonly ILogger _logger;

    public BoilerQueryHandler(
        IBoilerRepository boilerRepository,
        ILogger<BoilerQueryHandler> logger)
    {
        _boilerRepository = boilerRepository;
        _logger = logger;
    }

    public async Task<QueryResult<IReadOnlyCollection<GetBoilersResult>>> Handle(
        GetBoilersQuery request,
        CancellationToken cancellationToken)
    {
        var boilers = await _boilerRepository.GetAllAsync(cancellationToken);
        if (boilers.Count == 0)
        {
            _logger.LogInformation("There are no boilers registered.");
            return "There are no boilers registered.";
        }

        var boilersResult = boilers.Select(boiler =>
            new GetBoilersResult(
                boiler.Id,
                boiler.Name,
                boiler.State))
            .ToList();

        return boilersResult;
    }

    public async Task<QueryResult<GetBoilerResult>> Handle(
        GetBoilerQuery request,
        CancellationToken cancellationToken)
    {
        var boiler = await _boilerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (boiler is null)
        {
            _logger.LogWarning("Boiler with id {Id} does not exist", request.Id);
            return $"Boiler with id {request.Id} does not exist.";
        }

        var boilerResult = new GetBoilerResult(
            boiler.Id,
            boiler.Name,
            boiler.State,
            boiler.CurrentTemperature.Value);

        return boilerResult;
    }
}
