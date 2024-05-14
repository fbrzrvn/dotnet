using ErrorOr;
using FreeStuff.Items.Application.Shared.Dto;
using MediatR;

namespace FreeStuff.Items.Application.Search;

public record SearchItemsQuery(
    string? Title,
    string? CategoryName,
    string? Condition,
    string? SortBy
) : IRequest<ErrorOr<List<ItemDto>>>;
