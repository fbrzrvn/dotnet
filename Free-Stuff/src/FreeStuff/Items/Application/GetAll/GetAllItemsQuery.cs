using ErrorOr;
using FreeStuff.Items.Application.Shared.Dto;
using MediatR;

namespace FreeStuff.Items.Application.GetAll;

public record GetAllItemsQuery(int Page, int Limit) : IRequest<ErrorOr<ItemsDto>>;
