namespace Shared.Infrastructure.Security;

using Application.Security;
using CurrentUserProvider;
using ErrorOr;
using PolicyEnforcer;

public class AuthorizationService : IAuthorizationService
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IPolicyEnforcer      _policyEnforcer;

    public AuthorizationService(IPolicyEnforcer policyEnforcer, ICurrentUserProvider currentUserProvider)
    {
        _policyEnforcer      = policyEnforcer;
        _currentUserProvider = currentUserProvider;
    }

    public ErrorOr<Success> AuthorizeCurrentUser<T>(
        IAuthorizableRequest<T> request,
        IEnumerable<string>     requiredRoles,
        IEnumerable<string>     requiredPolicies
    )
    {
        var currentUser = _currentUserProvider.GetCurrentUser();

        if (requiredRoles.Except(currentUser.Roles).Any())
        {
            return Error.Failure(description: "User is missing required roles for taking this action");
        }

        foreach (var policy in requiredPolicies)
        {
            var authorizationAgainstPolicyResult = _policyEnforcer.Authorize(request, currentUser, policy);

            if (authorizationAgainstPolicyResult.IsError)
            {
                return authorizationAgainstPolicyResult.Errors;
            }
        }

        return Result.Success;
    }
}
