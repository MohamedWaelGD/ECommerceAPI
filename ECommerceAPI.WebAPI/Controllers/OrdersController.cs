using ECommerceAPI.Application.Common.Models;
using ECommerceAPI.Application.Features.Orders.Dtos;
using ECommerceAPI.Application.Features.Orders.Queries.GetMyOrders;
using ECommerceAPI.Application.Features.Orders.Queries.GetOrderById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.WebAPI.Controllers;

[Route("api/orders")]
[Authorize]
[Produces("application/json")]
public class OrdersController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PaginatedResponse<OrderDto>>> GetOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default) =>
        FromResult(await mediator.Send(new GetMyOrdersQuery(page, pageSize), cancellationToken));

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> GetOrder(Guid id, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new GetOrderByIdQuery(id), cancellationToken));
}
