namespace Identity.Domain;

using Shared.Domain;

public class IdentityUserX : AggregateRoot<IdentityUserId>
{
    public string   Username        { get; set; }
    public string   Email           { get; set; }
    public string   Password        { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
}
