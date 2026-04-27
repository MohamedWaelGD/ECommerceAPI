using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Cart.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Cart.Commands.UpdateCartItem;

public class UpdateCartItemCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<UpdateCartItemCommand, Result<CartDto>>
{
    public async Task<Result<CartDto>> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId is null) return Result<CartDto>.Failure("User is not authenticated.");

        var cart = await unitOfWork.Carts.GetOrCreateByUserIdAsync(userId.Value, cancellationToken);
        try
        {
            cart.UpdateItem(request.ItemId, request.Quantity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            return Result<CartDto>.Failure(ex.Message);
        }

        return Result<CartDto>.Success(cart.ToDto());
    }
}
