using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Common.Models;
using ECommerceAPI.Application.Features.Orders.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Orders.Queries.GetMyOrders;

public sealed record GetMyOrdersQuery(int Page = 1, int PageSize = 20) : IRequest<Result<PaginatedResponse<OrderDto>>>;
