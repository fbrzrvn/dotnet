using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Movies.Api.Auth;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyAuthFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        const string objectValue = "Missing API key";

        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractApiKey))
        {
            context.Result = new UnauthorizedObjectResult(objectValue);

            return;
        }

        var apiKey = _configuration["ApiKey"]!;

        if (apiKey != extractApiKey)
        {
            context.Result = new UnauthorizedObjectResult(objectValue);
        }
    }
}
