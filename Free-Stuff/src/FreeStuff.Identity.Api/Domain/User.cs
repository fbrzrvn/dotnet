using Microsoft.AspNetCore.Identity;

namespace FreeStuff.Identity.Api.Domain;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName  { get; set; } = string.Empty;
}
