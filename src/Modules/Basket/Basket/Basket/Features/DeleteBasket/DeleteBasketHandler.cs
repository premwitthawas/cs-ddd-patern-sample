namespace Basket.Basket.Features.DeleteBasket;

public record DeleteBasketCommand(string Username) : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool IsSuccess);
internal class DeleteBasketHandler(IBasketRepository repository)
: ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var result = await repository.DeleteBasket(command.Username, cancellationToken);
        return new DeleteBasketResult(result);
    }
};