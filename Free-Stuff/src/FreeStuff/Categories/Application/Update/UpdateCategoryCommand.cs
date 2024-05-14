using ErrorOr;
using FreeStuff.Categories.Application.Shared.Dto;
using MediatR;

namespace FreeStuff.Categories.Application.Update;

public record UpdateCategoryCommand(string Name, string NewName, string Description) : IRequest<ErrorOr<CategoryDto>>;
