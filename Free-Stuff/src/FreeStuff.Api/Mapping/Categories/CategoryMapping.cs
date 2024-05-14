using FreeStuff.Categories.Application.Shared.Dto;
using FreeStuff.Contracts.Categories.Responses;
using Mapster;

namespace FreeStuff.Api.Mapping.Categories;

public class CategoryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CategoryDto, CategoryResponse>();

        config.NewConfig<List<CategoryDto>, List<CategoryResponse>>();
    }
}
