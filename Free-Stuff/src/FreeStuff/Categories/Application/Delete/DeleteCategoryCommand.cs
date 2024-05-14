using ErrorOr;
using MediatR;

namespace FreeStuff.Categories.Application.Delete;

public record DeleteCategoryCommand(string Name) : IRequest<ErrorOr<bool>>;
