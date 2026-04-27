using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Application.Common.Interfaces.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByIdForUserAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<Order?> GetLatestPendingByUserIdWithItemsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Order?> GetByStripeSessionIdWithItemsAsync(string stripeSessionId, CancellationToken cancellationToken = default);
}
