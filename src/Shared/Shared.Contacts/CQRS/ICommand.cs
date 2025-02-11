using MediatR;

namespace Shared.Contacts.CQRS;
public interface ICommand : IRequest<Unit>
{
    
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}