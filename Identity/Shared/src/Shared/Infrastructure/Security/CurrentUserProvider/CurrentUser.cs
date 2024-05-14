namespace Shared.Infrastructure.Security.CurrentUserProvider;

public record CurrentUser(Guid Id, IReadOnlyList<string> Roles);
