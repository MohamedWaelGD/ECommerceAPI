using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Application.Common.Interfaces.Repositories;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetByUserIdWithItemsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Cart> GetOrCreateByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
