using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Application.Common.Models;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Orders.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application.Features.Orders.Queries.GetMyOrders;

public class GetMyOrdersQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<GetMyOrdersQuery, Result<PaginatedResponse<OrderDto>>>
{
    public async Task<Result<PaginatedResponse<OrderDto>>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId is null) return Result<PaginatedResponse<OrderDto>>.Unauthorized("User is not authenticated.");

        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var query = unitOfWork.Orders.Query()
            .AsNoTracking()
            .Include(x => x.Items)
            .Where(x => x.UserId == userId);

        var totalCount = await query.CountAsync(cancellationToken);
        var orders = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        return Result<PaginatedResponse<OrderDto>>.Success(new PaginatedResponse<OrderDto>(orders, page, pageSize, totalCount, totalPages));
    }
}
