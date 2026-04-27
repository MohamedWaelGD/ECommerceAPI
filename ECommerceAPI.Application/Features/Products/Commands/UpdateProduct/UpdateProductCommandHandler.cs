using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Products.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductCommand, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product is null) return Result<ProductDto>.Failure("Product not found.");

        product.Update(request.Name, request.Description, request.Price, request.StockQuantity, request.ImageUrl, request.IsActive);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<ProductDto>.Success(product.ToDto());
    }
}
