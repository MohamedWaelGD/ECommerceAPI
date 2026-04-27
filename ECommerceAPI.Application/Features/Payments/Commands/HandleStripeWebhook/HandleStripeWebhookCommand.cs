using ECommerceAPI.Application.Common.Results;
using MediatR;

namespace ECommerceAPI.Application.Features.Payments.Commands.HandleStripeWebhook;

public sealed record HandleStripeWebhookCommand(string Payload, string Signature) : IRequest<Result>;
