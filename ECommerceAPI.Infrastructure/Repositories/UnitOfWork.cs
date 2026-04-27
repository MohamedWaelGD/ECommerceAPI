using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Infrastructure.Persistence;

namespace ECommerceAPI.Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private IRepository<Product>? _products;
    private ICartRepository? _carts;
    private IOrderRepository? _orders;
    private IRepository<RefreshToken>? _refreshTokens;

    public IRepository<Product> Products => _products ??= new Repository<Product>(context);
    public ICartRepository Carts => _carts ??= new CartRepository(context);
    public IOrderRepository Orders => _orders ??= new OrderRepository(context);
    public IRepository<RefreshToken> RefreshTokens => _refreshTokens ??= new Repository<RefreshToken>(context);

    public void ClearChanges() => context.ChangeTracker.Clear();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => context.SaveChangesAsync(cancellationToken);
}
