using System.Linq.Expressions;
using DMS.Monitor.Domain.Base;

namespace DMS.Monitor.Domain.Persistence;

public interface IGenericRepository<TEntity> where TEntity : Entity
{
    Task<IEnumerable<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(Guid id);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
