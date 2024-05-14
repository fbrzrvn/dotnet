namespace Shared.Application.Security;

using ErrorOr;

public interface IAuthorizationService
{
    ErrorOr<Success> AuthorizeCurrentUser<T>(
        IAuthorizableRequest<T> request,
        IEnumerable<string>     requiredRoles,
        IEnumerable<string>     requiredPolicies
    );
}
