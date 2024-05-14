using ErrorOr;
using MediatR;

namespace FreeStuff.Items.Application.Delete;

public record DeleteItemCommand(Guid Id) : IRequest<ErrorOr<bool>>;
