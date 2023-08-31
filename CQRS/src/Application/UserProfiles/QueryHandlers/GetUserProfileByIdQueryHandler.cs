using Application.UserProfiles.Queries;
using Domain.Aggregates.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserProfiles.QueryHandlers;

internal class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, OperationResult<UserProfile>>
{
    private readonly DataContext _ctx;

    public GetUserProfileByIdQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<OperationResult<UserProfile>> Handle(GetUserProfileByIdQuery request,
        CancellationToken cancellationToken)
    {
        OperationResult<UserProfile> result = new();

        try
        {
            UserProfile? userProfile =
                await _ctx.UserProfiles.FirstOrDefaultAsync(
                    userProfile => userProfile.UserProfileId == request.UserProfileId, cancellationToken);

            if (userProfile is null)
            {
                result.IsError = true;
                result.Errors.Add(
                    new Error
                    {
                        Code = ErrorCode.NotFound,
                        Message = $"No user found with profile Id: {request.UserProfileId}"
                    }
                );

                return result;
            }

            result.Payload = userProfile;
        }
        catch (Exception ex)
        {
            result.IsError = true;
            result.Errors.Add(
                new Error { Code = ErrorCode.InternalServerError, Message = ex.Message }
            );
        }

        return result;
    }
}