using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Cart.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application.Features.Cart.Commands.AddCartItem;

public class AddCartItemCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<AddCartItemCommand, Result<CartDto>>
{
    public async Task<Result<CartDto>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId is null) return Result<CartDto>.Unauthorized("User is not authenticated.");

        for (var attempt = 0; attempt < 2; attempt++)
        {
            var product = await unitOfWork.Products.FirstOrDefaultAsync(x => x.Id == request.ProductId && x.IsActive, cancellationToken);
            if (product is null) return Result<CartDto>.NotFound("Product not found.");

            var cart = await unitOfWork.Carts.GetOrCreateByUserIdAsync(userId.Value, cancellationToken);
            try
            {
                cart.AddItem(product, request.Quantity);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                return Result<CartDto>.Success(cart.ToDto());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (attempt > 0) return Result<CartDto>.Conflict("Cart was modified by another request. Please retry.");
                unitOfWork.ClearChanges();
            }
            catch (InvalidOperationException ex)
            {
                return Result<CartDto>.Failure(ex.Message);
            }
        }

        return Result<CartDto>.Conflict("Cart was modified by another request. Please retry.");
    }
}
