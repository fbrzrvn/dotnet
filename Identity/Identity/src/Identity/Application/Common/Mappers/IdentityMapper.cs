namespace Identity.Application.Common.Mappers;

using System.Security.Claims;
using Commands.ChangePassword;
using Commands.RefreshToken;
using Commands.Update;
using DTOs;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Shared.Presentation.Contracts.Identity.Requests;

public class IdentityMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<(string AccessToken, RefreshTokenRequest Request), RefreshTokenCommand>()
            .Map(dest => dest.AccessToken, src => src.AccessToken.Substring(7))
            .Map(dest => dest, src => src.Request);

        config
            .NewConfig<(ClaimsPrincipal Claims, ChangePasswordRequest Request), ChangePasswordCommand>()
            .Map(dest => dest.Claims, src => src.Claims)
            .Map(dest => dest, src => src.Request);

        config
            .NewConfig<(string UserId, UpdateIdentityUserRequest Request), UpdateIdentityUserCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request);

        config
            .NewConfig<(IdentityUser User, string Role), IdentityUserDto>()
            .Map(dest => dest.Role, src => src.Role)
            .Map(dest => dest, src => src.User);

        config
            .NewConfig<(string AccessToken, string RefreshToken), AccessCredentialsDto>()
            .Map(dest => dest.AccessToken, src => src.AccessToken)
            .Map(dest => dest.RefreshToken, src => src.RefreshToken);
    }
}
