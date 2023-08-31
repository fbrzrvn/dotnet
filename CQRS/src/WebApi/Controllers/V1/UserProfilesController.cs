using Application;
using Application.UserProfiles.Commands;
using Application.UserProfiles.Queries;
using AutoMapper;
using Domain.Aggregates.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.UserProfiles.Requests;
using WebApi.Contracts.UserProfiles.Responses;

namespace WebApi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route(ApiEndpoints.Base)]
public class UserProfilesController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediatr;

    public UserProfilesController(IMediator mediatr, IMapper mapper)
    {
        _mediatr = mediatr;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserProfileRequest userProfileRequest, CancellationToken
        cancellationToken = default)
    {
        CreateUserProfileCommand? command = _mapper.Map<CreateUserProfileCommand>(userProfileRequest);

        UserProfile response = await _mediatr.Send(command, cancellationToken);

        UserProfileResponse? userProfile = _mapper.Map<UserProfileResponse>(response);

        return CreatedAtAction(nameof(GetById), new { id = response.UserProfileId }, userProfile);
    }

    [HttpGet(ApiEndpoints.Id)]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        GetUserProfileByIdQuery query = new() { UserProfileId = Guid.Parse(id) };

        OperationResult<UserProfile> response = await _mediatr.Send(query);

        if (response.IsError)
        {
            return HandleErrorResponse(response.Errors);
        }

        UserProfileResponse? userProfile = _mapper.Map<UserProfileResponse>(response);

        return Ok(userProfile);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        GetAllUserProfilesQuery query = new();

        IEnumerable<UserProfile> response = await _mediatr.Send(query);

        List<UserProfileResponse>? profiles = _mapper.Map<List<UserProfileResponse>>(response);

        return Ok(profiles);
    }

    [HttpPatch(ApiEndpoints.Id)]
    public async Task<IActionResult> Update([FromRoute] string id, UserProfileRequest userProfileRequest)
    {
        UpdateUserProfileCommand? command = _mapper.Map<UpdateUserProfileCommand>(userProfileRequest);
        command.UserProfileId = Guid.Parse(id);

        OperationResult<UserProfile> result = await _mediatr.Send(command);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpDelete(ApiEndpoints.Id)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        DeleteUserProfileCommand command = new() { UserProfileId = Guid.Parse(id) };

        OperationResult<UserProfile> result = await _mediatr.Send(command);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }
}