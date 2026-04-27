using ECommerceAPI.Application.Features.Payments.Commands.HandleStripeWebhook;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.WebAPI.Controllers;

[Route("api/payments")]
public class PaymentsController(IMediator mediator) : ApiControllerBase
{
    [HttpPost("stripe/webhook")]
    [AllowAnonymous]
    public async Task<ActionResult> StripeWebhook(CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Request.Body);
        var payload = await reader.ReadToEndAsync(cancellationToken);
        var signature = Request.Headers["Stripe-Signature"].ToString();
        return FromResult(await mediator.Send(new HandleStripeWebhookCommand(payload, signature), cancellationToken));
    }
}
