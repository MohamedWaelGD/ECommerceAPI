using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Application.Common.Results;
using MediatR;

namespace ECommerceAPI.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product is null) return Result.NotFound("Product not found.");

        product.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
