using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Cart.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Cart.Queries.GetMyCart;

public sealed record GetMyCartQuery : IRequest<Result<CartDto>>;
