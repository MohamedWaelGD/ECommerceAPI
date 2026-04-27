using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Cart.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Cart.Commands.ClearCart;

public sealed record ClearCartCommand : IRequest<Result<CartDto>>;
