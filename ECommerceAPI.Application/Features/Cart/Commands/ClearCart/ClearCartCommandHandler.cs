using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Cart.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application.Features.Cart.Commands.ClearCart;

public class ClearCartCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<ClearCartCommand, Result<CartDto>>
{
    public async Task<Result<CartDto>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId is null) return Result<CartDto>.Unauthorized("User is not authenticated.");

        for (var attempt = 0; attempt < 2; attempt++)
        {
            var cart = await unitOfWork.Carts.GetOrCreateByUserIdAsync(userId.Value, cancellationToken);
            try
            {
                cart.Clear();
                await unitOfWork.SaveChangesAsync(cancellationToken);
                return Result<CartDto>.Success(cart.ToDto());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (attempt > 0) return Result<CartDto>.Conflict("Cart was modified by another request. Please retry.");
                unitOfWork.ClearChanges();
            }
        }

        return Result<CartDto>.Conflict("Cart was modified by another request. Please retry.");
    }
}
