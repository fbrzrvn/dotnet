namespace Shared.Application.Behaviors;

using System.Reflection;
using ErrorOr;
using MediatR;
using Security;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAuthorizableRequest<TResponse> where TResponse : IErrorOr
{
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationBehavior(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<TResponse> Handle(
        TRequest                          request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken                 cancellationToken
    )
    {
        var authorizationAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>().ToList();

        if (authorizationAttributes.Count == 0)
        {
            return await next();
        }

        var requiredRoles = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Roles?.Split(',') ?? [])
            .ToList();

        var requiredPolicies = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Policies?.Split(',') ?? [])
            .ToList();

        var authorizationResult = _authorizationService.AuthorizeCurrentUser(request, requiredRoles, requiredPolicies);

        return authorizationResult.IsError ? (dynamic)authorizationResult.Errors : await next();
    }
}
