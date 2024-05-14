using ErrorOr;
using FreeStuff.Categories.Application.Shared.Dto;
using MediatR;

namespace FreeStuff.Categories.Application.Get;

public record GetCategoryQuery(string Name) : IRequest<ErrorOr<CategoryDto>>;
