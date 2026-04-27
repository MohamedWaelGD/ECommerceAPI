using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Cart.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Cart.Commands.ClearCart;

public class ClearCartCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<ClearCartCommand, Result<CartDto>>
{
    public async Task<Result<CartDto>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId is null) return Result<CartDto>.Failure("User is not authenticated.");

        var cart = await unitOfWork.Carts.GetOrCreateByUserIdAsync(userId.Value, cancellationToken);
        cart.Clear();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<CartDto>.Success(cart.ToDto());
    }
}
