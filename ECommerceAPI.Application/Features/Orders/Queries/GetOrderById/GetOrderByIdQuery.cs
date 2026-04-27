using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Orders.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid Id) : IRequest<Result<OrderDto>>;
