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
[Produces("application/json")]
public class CartController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CartDto>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default) =>
        FromResult(await mediator.Send(new GetMyCartQuery(page, pageSize), cancellationToken));

    [HttpPost("items")]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CartDto>> AddItem(AddCartItemCommand command, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(command, cancellationToken));

    [HttpPut("items/{itemId:guid}")]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CartDto>> UpdateItem(Guid itemId, UpdateCartItemRequest request, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new UpdateCartItemCommand(itemId, request.Quantity), cancellationToken));

    [HttpDelete("items/{itemId:guid}")]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CartDto>> RemoveItem(Guid itemId, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new RemoveCartItemCommand(itemId), cancellationToken));

    [HttpDelete("clear")]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CartDto>> Clear(CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new ClearCartCommand(), cancellationToken));
}

public sealed record UpdateCartItemRequest(int Quantity);
