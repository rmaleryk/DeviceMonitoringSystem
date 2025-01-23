using DMS.Monitor.Domain.Boilers.Enums;
using MediatR;

namespace DMS.Monitor.Application.Read.Boilers.Queries;

public sealed record class GetBoilerQuery(Guid Id) : IRequest<QueryResult<GetBoilerResult>>;

public sealed record class GetBoilerResult(Guid Id, string Name, BoilerState State, double CurrentTemperature);
