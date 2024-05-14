namespace FreeStuff.Items.Application.Shared.Dto;

public class ItemsDto
{
    public List<ItemDto> Data        { get; }
    public int           TotalResult { get; }

    public ItemsDto(List<ItemDto> data, int totalResult)
    {
        Data        = data;
        TotalResult = totalResult;
    }
}
