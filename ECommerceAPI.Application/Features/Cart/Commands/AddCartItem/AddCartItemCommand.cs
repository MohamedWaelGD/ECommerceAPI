using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Cart.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Cart.Commands.AddCartItem;

public sealed record AddCartItemCommand(Guid ProductId, int Quantity) : IRequest<Result<CartDto>>;
