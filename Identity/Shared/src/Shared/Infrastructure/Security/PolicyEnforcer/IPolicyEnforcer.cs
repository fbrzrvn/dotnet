namespace Shared.Infrastructure.Security.PolicyEnforcer;

using Application.Security;
using CurrentUserProvider;
using ErrorOr;

public interface IPolicyEnforcer
{
    public ErrorOr<Success> Authorize<T>(IAuthorizableRequest<T> request, CurrentUser currentUser, string policy);
}
