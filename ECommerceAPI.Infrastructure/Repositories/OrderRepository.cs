using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Infrastructure.Repositories;

public class OrderRepository(ApplicationDbContext context) : Repository<Order>(context), IOrderRepository
{
    public Task<Order?> GetByIdForUserAsync(Guid id, Guid userId, CancellationToken cancellationToken = default) =>
        Context.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public Task<Order?> GetByStripeSessionIdWithItemsAsync(string stripeSessionId, CancellationToken cancellationToken = default) =>
        Context.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.StripeCheckoutSessionId == stripeSessionId, cancellationToken);
}
