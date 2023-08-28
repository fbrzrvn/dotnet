using Application.UserProfiles.Commands;
using AutoMapper;
using Domain.Aggregates.User;
using WebApi.Contracts.UserProfiles.Requests;
using WebApi.Contracts.UserProfiles.Responses;

namespace WebApi.Mappers;

public class UserProfileMappers : Profile
{
    public UserProfileMappers()
    {
        CreateMap<UserProfileRequest, CreateUserProfileCommand>();

        CreateMap<UserProfileRequest, UpdateUserProfileCommand>();

        CreateMap<UserProfile, UserProfileResponse>();

        CreateMap<UserInfo, UserInfoResponse>();
    }
}