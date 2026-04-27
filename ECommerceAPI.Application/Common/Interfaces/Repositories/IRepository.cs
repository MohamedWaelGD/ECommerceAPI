using System.Linq.Expressions;

namespace ECommerceAPI.Application.Common.Interfaces.Repositories;

public interface IRepository<TEntity>
    where TEntity : class
{
    IQueryable<TEntity> Query();
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(TEntity entity);
    void Remove(TEntity entity);
}
