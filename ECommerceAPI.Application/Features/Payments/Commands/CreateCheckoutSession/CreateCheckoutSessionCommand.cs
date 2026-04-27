using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Payments.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Payments.Commands.CreateCheckoutSession;

public sealed record CreateCheckoutSessionCommand : IRequest<Result<CheckoutResponse>>;
