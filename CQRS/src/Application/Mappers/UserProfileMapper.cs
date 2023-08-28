using Application.UserProfiles.Commands;
using AutoMapper;
using Domain.Aggregates.User;

namespace Application.Mappers;

public class UserProfileMapper : Profile
{
    public UserProfileMapper()
    {
        CreateMap<CreateUserProfileCommand, UserInfo>();
    }
}