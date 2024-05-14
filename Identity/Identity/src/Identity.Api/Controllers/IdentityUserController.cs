namespace Identity.Api.Controllers;

using Application.Commands.Create;
using Application.Commands.Delete;
using Application.Commands.Update;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Presentation.Contracts.Identity.Requests;
using Shared.Presentation.Contracts.Identity.Responses;

[Authorize]
public class IdentityUserController : ApiController
{
    private readonly ISender _bus;
    private readonly IMapper _mapper;

    public IdentityUserController(ISender bus, IMapper mapper)
    {
        _bus    = bus;
        _mapper = mapper;
    }

    [HttpPost(ApiEndpoints.Identity.User.Create)]
    public async Task<IActionResult> CreateIdentityUser(
        [FromBody] CreateIdentityUserRequest request,
        CancellationToken                    cancellationToken
    )
    {
        var command = _mapper.Map<CreateIdentityUserCommand>(request);
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(
            user => Created($"/api/users/{user.Id}", _mapper.Map<IdentityUserResponse>(user)),
            errors => Problem(errors)
        );
    }

    [HttpPut(ApiEndpoints.Identity.User.Update)]
    public async Task<IActionResult> UpdateIdentityUser(
        [FromRoute] string                    id,
        [FromBody]  UpdateIdentityUserRequest request,
        CancellationToken                     cancellationToken
    )
    {
        var command = _mapper.Map<UpdateIdentityUserCommand>((id, request));
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), errors => Problem(errors));
    }

    [HttpDelete(ApiEndpoints.Identity.User.Delete)]
    public async Task<IActionResult> DeleteIdentityUser([FromRoute] string id, CancellationToken cancellationToken)
    {
        var command = new DeleteIdentityUserCommand(id);
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), errors => Problem(errors));
    }
}
