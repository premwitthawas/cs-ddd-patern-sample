using MediatR;

namespace Shared.Contacts.CQRS;

public interface IQuery<out T>: IRequest<T>
    where T : notnull
{

}