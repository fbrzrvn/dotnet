using FluentValidation;
using Movies.Contracts.Responses;

namespace Movies.Api.Mappers;

public class ValidationMapperMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMapperMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 400;

            var validationFailureResponse = new ValidationFailureResponse
            {
                Errors = ex.Errors.Select(x => new ValidationResponse
                {
                    PropertyName = x.PropertyName,
                    Message = x.ErrorMessage,
                })
            };

            await context.Response.WriteAsJsonAsync(validationFailureResponse);
        }
    }
}
