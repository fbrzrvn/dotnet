using Application.UserProfiles.Commands;
using Domain.Aggregates.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserProfiles.CommandHandlers;

internal class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand, OperationResult<UserProfile>>
{
    private readonly DataContext _ctx;

    public DeleteUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<OperationResult<UserProfile>> Handle(DeleteUserProfileCommand request,
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

            _ctx.UserProfiles.Remove(userProfile!);
            await _ctx.SaveChangesAsync(cancellationToken);
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