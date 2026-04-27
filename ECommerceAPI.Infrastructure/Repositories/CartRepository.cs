using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Infrastructure.Repositories;

public class CartRepository(ApplicationDbContext context) : Repository<Cart>(context), ICartRepository
{
    public Task<Cart?> GetByUserIdWithItemsAsync(Guid userId, CancellationToken cancellationToken = default) =>
        Context.Carts
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

    public async Task<Cart> GetOrCreateByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var cart = await GetByUserIdWithItemsAsync(userId, cancellationToken);
        if (cart is not null) return cart;

        cart = Cart.Create(userId);
        Add(cart);
        return cart;
    }
}
