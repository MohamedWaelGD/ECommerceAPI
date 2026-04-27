using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Orders.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
{
    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId is null) return Result<OrderDto>.Unauthorized("User is not authenticated.");

        var order = await unitOfWork.Orders.GetByIdForUserAsync(request.Id, userId.Value, cancellationToken);
        return order is null ? Result<OrderDto>.NotFound("Order not found.") : Result<OrderDto>.Success(order.ToDto());
    }
}
