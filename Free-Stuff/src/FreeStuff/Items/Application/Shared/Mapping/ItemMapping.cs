using FreeStuff.Contracts.Items.Events;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Domain;
using Mapster;

namespace FreeStuff.Items.Application.Shared.Mapping;

public class ItemMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Item, ItemDto>()
              .Map(dest => dest.Id, src => src.Id.Value)
              .Map(dest => dest.CategoryName, src => src.Category.Name)
              .Map(dest => dest.Condition, src => src.Condition.MapItemConditionToString())
              .Map(dest => dest.UserId, src => src.UserId.Value);

        config.NewConfig<Item, ItemCreated>()
              .Map(dest => dest.Id, src => src.Id.Value)
              .Map(dest => dest.Category, src => src.Category.Name)
              .Map(dest => dest.Condition, src => src.Condition.MapItemConditionToString())
              .Map(dest => dest.UserId, src => src.UserId.Value);

        config.NewConfig<Item, ItemUpdated>()
              .Map(dest => dest.Id, src => src.Id.Value)
              .Map(dest => dest.Category, src => src.Category.Name)
              .Map(dest => dest.Condition, src => src.Condition.MapItemConditionToString())
              .Map(dest => dest.UserId, src => src.UserId.Value);
    }
}
