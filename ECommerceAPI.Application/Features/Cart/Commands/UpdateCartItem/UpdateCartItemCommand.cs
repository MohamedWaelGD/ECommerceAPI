using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Cart.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Cart.Commands.UpdateCartItem;

public sealed record UpdateCartItemCommand(Guid ItemId, int Quantity) : IRequest<Result<CartDto>>;
