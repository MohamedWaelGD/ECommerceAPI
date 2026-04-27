using ECommerceAPI.Application.Features.Products.Commands.CreateProduct;
using ECommerceAPI.Application.Features.Products.Commands.DeleteProduct;
using ECommerceAPI.Application.Features.Products.Commands.UpdateProduct;
using ECommerceAPI.Application.Features.Products.Dtos;
using ECommerceAPI.Application.Features.Products.Queries.GetProductById;
using ECommerceAPI.Application.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.WebAPI.Controllers;

[Route("api/products")]
public class ProductsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyCollection<ProductDto>>> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default) =>
        FromResult(await mediator.Send(new GetProductsQuery(page, pageSize), cancellationToken));

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProductDto>> GetProduct(Guid id, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new GetProductByIdQuery(id), cancellationToken));

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDto>> Create(CreateProductCommand command, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(command, cancellationToken));

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDto>> Update(Guid id, UpdateProductRequest request, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new UpdateProductCommand(id, request.Name, request.Description, request.Price, request.StockQuantity, request.ImageUrl, request.IsActive), cancellationToken));

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new DeleteProductCommand(id), cancellationToken));
}

public sealed record UpdateProductRequest(string Name, string? Description, decimal Price, int StockQuantity, string? ImageUrl, bool IsActive = true);
