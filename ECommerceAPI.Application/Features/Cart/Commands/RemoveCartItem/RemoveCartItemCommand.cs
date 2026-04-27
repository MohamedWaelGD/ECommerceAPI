using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Cart.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Cart.Commands.RemoveCartItem;

public sealed record RemoveCartItemCommand(Guid ItemId) : IRequest<Result<CartDto>>;
