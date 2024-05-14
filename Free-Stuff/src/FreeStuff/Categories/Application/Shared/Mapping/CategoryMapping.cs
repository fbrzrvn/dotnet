using FreeStuff.Categories.Application.Shared.Dto;
using FreeStuff.Categories.Domain;
using Mapster;

namespace FreeStuff.Categories.Application.Shared.Mapping;

public class CategoryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Category, CategoryDto>()
              .Map(dest => dest.Id, src => src.Id.Value);
    }
}
