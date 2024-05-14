using ErrorOr;
using FreeStuff.Categories.Application.Shared.Dto;
using MediatR;

namespace FreeStuff.Categories.Application.Create;

public record CreateCategoryCommand
(
    string  Name,
    string? Description
) : IRequest<ErrorOr<CategoryDto>>;
