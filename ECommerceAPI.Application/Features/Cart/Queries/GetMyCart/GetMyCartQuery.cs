using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Cart.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Cart.Queries.GetMyCart;

public sealed record GetMyCartQuery(int Page = 1, int PageSize = 20) : IRequest<Result<CartDto>>;
