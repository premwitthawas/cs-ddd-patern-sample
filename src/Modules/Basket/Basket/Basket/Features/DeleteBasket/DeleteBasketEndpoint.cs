
namespace Basket.Basket.Features.DeleteBasket;

// public record DeleteBasketRequest(string UserName);
public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, ISender sender)=> {
            var command = new DeleteBasketCommand(userName);
            var result = await sender.Send(command);
            var response = result.Adapt<DeleteBasketResponse>();
            return Results.Ok(response);
        })
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete a basket")
        .WithDescription("Delete a basket for the user");
    }
}