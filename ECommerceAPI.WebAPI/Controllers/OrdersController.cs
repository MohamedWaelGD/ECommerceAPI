using ECommerceAPI.Application.Features.Orders.Dtos;
using ECommerceAPI.Application.Features.Orders.Queries.GetMyOrders;
using ECommerceAPI.Application.Features.Orders.Queries.GetOrderById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.WebAPI.Controllers;

[Route("api/orders")]
[Authorize]
public class OrdersController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<OrderDto>>> GetOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default) =>
        FromResult(await mediator.Send(new GetMyOrdersQuery(page, pageSize), cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetOrder(Guid id, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new GetOrderByIdQuery(id), cancellationToken));
}
