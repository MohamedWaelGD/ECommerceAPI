using ECommerceAPI.Application.Features.Cart.Commands.AddCartItem;
using ECommerceAPI.Application.Features.Cart.Commands.ClearCart;
using ECommerceAPI.Application.Features.Cart.Commands.RemoveCartItem;
using ECommerceAPI.Application.Features.Cart.Commands.UpdateCartItem;
using ECommerceAPI.Application.Features.Cart.Dtos;
using ECommerceAPI.Application.Features.Cart.Queries.GetMyCart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.WebAPI.Controllers;

[Route("api/cart")]
[Authorize]
public class CartController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CartDto>> Get(CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new GetMyCartQuery(), cancellationToken));

    [HttpPost("items")]
    public async Task<ActionResult<CartDto>> AddItem(AddCartItemCommand command, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(command, cancellationToken));

    [HttpPut("items/{itemId:guid}")]
    public async Task<ActionResult<CartDto>> UpdateItem(Guid itemId, UpdateCartItemRequest request, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new UpdateCartItemCommand(itemId, request.Quantity), cancellationToken));

    [HttpDelete("items/{itemId:guid}")]
    public async Task<ActionResult<CartDto>> RemoveItem(Guid itemId, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new RemoveCartItemCommand(itemId), cancellationToken));

    [HttpDelete("clear")]
    public async Task<ActionResult<CartDto>> Clear(CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new ClearCartCommand(), cancellationToken));
}

public sealed record UpdateCartItemRequest(int Quantity);
