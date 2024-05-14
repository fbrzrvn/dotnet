namespace Identity.Api.Controllers;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class ErrorsController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)] // fix: Ambiguous HTTP method for action
    [Route(ApiEndpoints.Errors)]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return Problem(
            exception?.Message ?? "An error occurred while processing your request",
            title: "Internal Server Error",
            statusCode: 500
        );
    }
}
