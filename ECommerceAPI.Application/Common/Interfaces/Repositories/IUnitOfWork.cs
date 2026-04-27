using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Application.Common.Interfaces.Repositories;

public interface IUnitOfWork
{
    IRepository<Product> Products { get; }
    ICartRepository Carts { get; }
    IOrderRepository Orders { get; }
    IRepository<RefreshToken> RefreshTokens { get; }
    void ClearChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
