namespace Shared.Application.Security;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AuthorizeAttribute : Attribute
{
    public string? Roles    { get; set; }
    public string? Policies { get; set; }
}
