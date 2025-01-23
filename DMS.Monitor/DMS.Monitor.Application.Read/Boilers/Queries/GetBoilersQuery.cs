using DMS.Monitor.Domain.Boilers.Enums;
using MediatR;

namespace DMS.Monitor.Application.Read.Boilers.Queries;

public sealed record class GetBoilersQuery() : IRequest<QueryResult<IReadOnlyCollection<GetBoilersResult>>>;

public sealed record class GetBoilersResult(Guid Id, string Name, BoilerState State);
