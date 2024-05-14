using FreeStuff.Contracts.Identity.Requests;
using FreeStuff.Identity.Api.Domain;
using Mapster;

namespace FreeStuff.Identity.Api.Mapping;

public class IdentityMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterUserRequest, User>();
    }
}
