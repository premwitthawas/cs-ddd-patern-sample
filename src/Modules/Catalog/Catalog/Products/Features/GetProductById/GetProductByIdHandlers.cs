namespace Catalog.Products.Features.GetProductById;
internal class GetProductByIdHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
        .AsNoTracking()
        .SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken);
        if (product is null)
        {
            throw new Exception($"Product with Id {query.Id} not found");
        }
        var productDto = product.Adapt<ProductDto>();
        return new GetProductByIdResult(productDto);
    }
}