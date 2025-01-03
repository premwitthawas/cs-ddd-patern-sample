namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(ProductDto ProductDto) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductHandler(CatalogDbContext dbContext) : ICommandHandler<CreateProductCommand,
    CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = CreateNewProduct(command.ProductDto);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CreateProductResult(product.Id);
    }

    private Product CreateNewProduct(ProductDto productDto)
    {
        var product = Product.Create(
            Guid.NewGuid(),
            productDto.Name,
            productDto.Category,
            productDto.Description,
            productDto.ImageFile,
            productDto.Price);

        return product;
    }
}