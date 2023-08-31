using Application.UserProfiles.Commands;
using Domain.Aggregates.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserProfiles.CommandHandlers;

internal class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, OperationResult<UserProfile>>
{
    private readonly DataContext _ctx;

    public UpdateUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileCommand request,
        CancellationToken cancellationToken)
    {
        OperationResult<UserProfile> result = new();

        try
        {
            UserProfile? userProfile = await _ctx.UserProfiles.FirstOrDefaultAsync(userProfile =>
                userProfile.UserProfileId == request.UserProfileId, cancellationToken);

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

            UserInfo userInfo = UserInfo.CreateUserInfo(request.FirstName, request.LastName, request.EmailAddress,
                request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

            userProfile.UpdateUserInfo(userInfo);

            _ctx.UserProfiles.Update(userProfile);
            await _ctx.SaveChangesAsync(cancellationToken);

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