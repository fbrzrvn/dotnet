using FreeStuff.Contracts.Items.Requests;
using FreeStuff.Contracts.Items.Responses;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Application.Update;
using Mapster;

namespace FreeStuff.Api.Mapping.Items;

public class ItemMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(ItemsDto ItemsDto, GetAllItemsRequest Request), ItemsResponse>()
              .Map(dest => dest.Data, src => src.ItemsDto.Data)
              .Map(dest => dest.Page, src => src.Request.Page)
              .Map(dest => dest.Limit, src => src.Request.Limit)
              .Map(dest => dest.TotalResults, src => src.ItemsDto.TotalResult);

        config.NewConfig<(List<ItemDto> Items, SearchItemsRequest Request, int TotalResults), ItemsResponse>()
              .Map(dest => dest.Data, src => src.Items)
              .Map(dest => dest.Page, src => src.Request.Page)
              .Map(dest => dest.Limit, src => src.Request.Limit)
              .Map(dest => dest.TotalResults, src => src.TotalResults);

        config.NewConfig<(Guid Id, UpdateItemRequest Request), UpdateItemCommand>()
              .Map(dest => dest.Id, src => src.Id)
              .Map(dest => dest, src => src.Request);
    }
}
