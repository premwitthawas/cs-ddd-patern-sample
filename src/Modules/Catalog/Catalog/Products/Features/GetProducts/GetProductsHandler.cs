using Shared.PaginationHandlers;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery(PaginationRequest PaginationRequest) : IQuery<GetProductsResult> { };

public record GetProductsResult(PaginationResult<ProductDto> Products);

internal class GetProductsHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;
        var totalProducts = await dbContext.Products.LongCountAsync(cancellationToken);
        var products = await dbContext.Products
        .AsNoTracking()
        .OrderBy(p => p.Name)
        .Skip(pageIndex * pageSize)
        .Take(pageSize)
        .ToListAsync(cancellationToken);
        var productsDto = products.Adapt<List<ProductDto>>();
        return new GetProductsResult(
            new PaginationResult<ProductDto>(
                pageIndex,
                pageSize,
                totalProducts,
                productsDto
            )
        );
    }
};