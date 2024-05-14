namespace Identity.Api.Controllers;

using Application.Commands.ChangePassword;
using Application.Commands.ConfirmEmail;
using Application.Commands.ForgotPassword;
using Application.Commands.Login;
using Application.Commands.Logout;
using Application.Commands.RefreshToken;
using Application.Commands.Register;
using Application.Commands.ResetPassword;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Presentation.Contracts.Identity.Requests;
using Shared.Presentation.Contracts.Identity.Responses;

public class IdentityController : ApiController
{
    private readonly ISender _bus;
    private readonly IMapper _mapper;

    public IdentityController(ISender bus, IMapper mapper)
    {
        _bus    = bus;
        _mapper = mapper;
    }

    [HttpGet(ApiEndpoints.Identity.Welcome)]
    public OkObjectResult Welcome()
    {
        return Ok("Welcome to identity API");
    }

    [HttpPost(ApiEndpoints.Identity.Register)]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequest request,
        CancellationToken              cancellationToken
    )
    {
        var command = _mapper.Map<RegisterUserCommand>(request);
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(
            user => Created($"/api/users/{user.Id}", _mapper.Map<IdentityUserResponse>(user)),
            errors => Problem(errors)
        );
    }

    [HttpPost(ApiEndpoints.Identity.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmail(
        [FromBody] ConfirmEmailRequest request,
        CancellationToken              cancellationToken
    )
    {
        var command = _mapper.Map<ConfirmEmailCommand>(request);
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(_ => Ok("Email confirmed successfully"), errors => Problem(errors));
    }

    [HttpPost(ApiEndpoints.Identity.Login)]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<LoginUserCommand>(request);
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(
            accessCredentials => Ok(_mapper.Map<AccessCredentialsResponse>(accessCredentials)),
            errors => Problem(errors)
        );
    }

    [HttpPost(ApiEndpoints.Identity.RefreshToken)]
    public async Task<IActionResult> GenerateAccessCredentials(
        [FromBody] RefreshTokenRequest request,
        CancellationToken              cancellationToken
    )
    {
        var token   = HttpContext.Request.Headers.Authorization.ToString();
        var command = _mapper.Map<RefreshTokenCommand>((token, request));
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(
            accessCredentials => Ok(_mapper.Map<AccessCredentialsResponse>(accessCredentials)),
            errors => Problem(errors)
        );
    }

    [HttpPost(ApiEndpoints.Identity.ForgotPassword)]
    public async Task<IActionResult> GenerateResetPasswordToken(
        [FromBody] ForgotPasswordRequest request,
        CancellationToken                cancellationToken
    )
    {
        var command = _mapper.Map<ForgotPasswordCommand>(request);
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(token => Ok(token), errors => Problem(errors));
    }

    [HttpPost(ApiEndpoints.Identity.ResetPassword)]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordRequest request,
        CancellationToken               cancellationToken
    )
    {
        var command = _mapper.Map<ResetPasswordCommand>(request);
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(response => Ok(response), errors => Problem(errors));
    }

    [HttpPost(ApiEndpoints.Identity.ChangePassword)]
    [Authorize]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        CancellationToken                cancellationToken
    )
    {
        var command = _mapper.Map<ChangePasswordCommand>((User, request));
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(response => Ok(response), errors => Problem(errors));
    }

    [HttpPost(ApiEndpoints.Identity.Logout)]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var command = new LogoutUserCommand(User);
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(_ => Ok("Logout successful"), errors => Problem(errors));
    }
}
