using ErrorOr;
using FreeStuff.Items.Application.Shared.Dto;
using MediatR;

namespace FreeStuff.Items.Application.Create;

public record CreateItemCommand
(
    string Title,
    string Description,
    string CategoryName,
    string Condition,
    Guid   UserId
) : IRequest<ErrorOr<ItemDto>>;
