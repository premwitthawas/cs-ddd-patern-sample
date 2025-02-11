
using Shared.PaginationHandlers;

namespace Catalog.Products.Features.GetProducts;

// public record GetProductsResponse(IEnumerable<ProductDto> Products);

public record GetProductsResponse(PaginationResult<ProductDto> Products);
public class GetProductsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] PaginationRequest request, ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuery(request));
            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get all products")
        .WithDescription("Get all products from the catalog");
    }
}