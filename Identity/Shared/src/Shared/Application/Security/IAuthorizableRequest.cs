namespace Shared.Application.Security;

using MediatR;

public interface IAuthorizableRequest<T> : IRequest<T>
{
    string UserId { get; }
}
