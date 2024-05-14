namespace Shared.Infrastructure.Security.PolicyEnforcer;

using Application.Security;
using Application.Security.Constants;
using CurrentUserProvider;
using ErrorOr;

public class PolicyEnforcer : IPolicyEnforcer
{
    public ErrorOr<Success> Authorize<T>(IAuthorizableRequest<T> request, CurrentUser currentUser, string policy)
    {
        return policy switch
        {
            Policy.SelfOrAdmin   => SelfOrAdminPolicy(request, currentUser),
            Policy.MemberOrAdmin => MemberOrAdminPolicy(request, currentUser),
            _                    => Error.Unexpected(description: "Unknown policy name")
        };
    }

    private static ErrorOr<Success> SelfOrAdminPolicy<T>(IAuthorizableRequest<T> request, CurrentUser currentUser)
    {
        return request.UserId == currentUser.Id.ToString() || currentUser.Roles.Contains(Roles.Admin) ?
            Result.Success :
            Error.Failure(
                description:
                "Access Denied: You do not have permission to perform this action. This action is restricted to the user themselves or administrators"
            );
    }

    private static ErrorOr<Success> MemberOrAdminPolicy<T>(IAuthorizableRequest<T> _, CurrentUser currentUser)
    {
        return currentUser.Roles.Contains(Roles.Admin) || currentUser.Roles.Contains(Roles.Member) ?
            Result.Success :
            Error.Failure(
                description:
                "Access Denied: You do not have permission to perform this action. This action is restricted to administrators or members"
            );
    }
}
