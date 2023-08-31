using Application;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Common;

namespace WebApi.Controllers;

public class BaseController : ControllerBase
{
    protected IActionResult HandleErrorResponse(List<Error> errors)
    {
        ErrorResponse apiError = new();

        if (errors.Any(error => error.Code == ErrorCode.NotFound))
        {
            Error? error = errors.FirstOrDefault(error => error.Code == ErrorCode.NotFound);

            apiError.StatusCode = 404;
            apiError.Message = "Not Found";
            apiError.Errors.Add(error.Message);
            apiError.TimeStamp = DateTime.Now;

            return NotFound(apiError);
        }

        apiError.StatusCode = 500;
        apiError.Message = "Internal Server Error";
        apiError.Errors.Add("Unknown error");
        apiError.TimeStamp = DateTime.Now;

        return StatusCode(500, apiError);
    }
}