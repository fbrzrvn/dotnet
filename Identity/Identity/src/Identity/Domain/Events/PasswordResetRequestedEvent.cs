namespace Identity.Domain.Events;

using Microsoft.AspNetCore.Identity;
using Shared.Domain;

public record PasswordResetRequestedEvent(IdentityUser User, string Token) : IDomainEvent;
