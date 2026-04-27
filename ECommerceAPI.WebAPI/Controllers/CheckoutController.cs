using ECommerceAPI.Application.Features.Payments.Commands.CreateCheckoutSession;
using ECommerceAPI.Application.Features.Payments.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.WebAPI.Controllers;

[Route("api/checkout")]
[Authorize]
public class CheckoutController(IMediator mediator) : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CheckoutResponse>> Create(CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new CreateCheckoutSessionCommand(), cancellationToken));
}
