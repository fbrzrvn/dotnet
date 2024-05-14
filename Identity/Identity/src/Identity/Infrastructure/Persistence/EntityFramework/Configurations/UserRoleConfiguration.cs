namespace Identity.Infrastructure.Persistence.EntityFramework.Configurations;

using Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Name             = Role.Admin.Name,
                NormalizedName   = Role.Admin.NormalizedName,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new IdentityRole
            {
                Name             = Role.Member.Name,
                NormalizedName   = Role.Member.NormalizedName,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new IdentityRole
            {
                Name             = Role.RegularUser.Name,
                NormalizedName   = Role.RegularUser.NormalizedName,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        );
    }
}
