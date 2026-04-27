using ECommerceAPI.Application.Common.Results;
using MediatR;

namespace ECommerceAPI.Application.Features.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id) : IRequest<Result>;
