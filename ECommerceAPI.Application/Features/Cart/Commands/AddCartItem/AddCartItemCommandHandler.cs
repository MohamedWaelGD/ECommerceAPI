using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Cart.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Cart.Commands.AddCartItem;

public class AddCartItemCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<AddCartItemCommand, Result<CartDto>>
{
    public async Task<Result<CartDto>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId is null) return Result<CartDto>.Failure("User is not authenticated.");

        var product = await unitOfWork.Products.FirstOrDefaultAsync(x => x.Id == request.ProductId && x.IsActive, cancellationToken);
        if (product is null) return Result<CartDto>.Failure("Product not found.");

        var cart = await unitOfWork.Carts.GetOrCreateByUserIdAsync(userId.Value, cancellationToken);
        try
        {
            cart.AddItem(product, request.Quantity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            return Result<CartDto>.Failure(ex.Message);
        }

        return Result<CartDto>.Success(cart.ToDto());
    }
}
