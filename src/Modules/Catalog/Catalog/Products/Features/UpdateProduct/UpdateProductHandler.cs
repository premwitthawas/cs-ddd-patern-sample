
namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(ProductDto ProductDto) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

internal class UpdateProductHandler(CatalogDbContext dbContext) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ProductDto.Id], cancellationToken);
        if (product is null)
        {
            throw new Exception($"Product with id {command.ProductDto.Id} not found");
        }
        UpdateProductWithNewValues(product, command.ProductDto);
        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateProductResult(true);
    }

    private Product UpdateProductWithNewValues(Product product, ProductDto productDto)
    {
        product.Update(
            productDto.Name,
            productDto.Category,
            productDto.Description,
            productDto.ImageFile,
            productDto.Price);
        return product;
    }
}