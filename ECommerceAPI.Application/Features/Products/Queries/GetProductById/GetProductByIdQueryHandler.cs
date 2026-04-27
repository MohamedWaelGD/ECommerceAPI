using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.Query().AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);
        return product is null ? Result<ProductDto>.NotFound("Product not found.") : Result<ProductDto>.Success(product.ToDto());
    }
}
