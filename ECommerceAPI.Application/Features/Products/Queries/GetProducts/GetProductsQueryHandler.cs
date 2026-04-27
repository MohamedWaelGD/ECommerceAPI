using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Application.Common.Models;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProductsQuery, Result<PaginatedResponse<ProductDto>>>
{
    public async Task<Result<PaginatedResponse<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var query = unitOfWork.Products.Query()
            .AsNoTracking()
            .Where(x => x.IsActive);

        var totalCount = await query.CountAsync(cancellationToken);
        var products = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        return Result<PaginatedResponse<ProductDto>>.Success(new PaginatedResponse<ProductDto>(products, page, pageSize, totalCount, totalPages));
    }
}
