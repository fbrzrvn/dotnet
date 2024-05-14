using FreeStuff.Identity.Api.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreeStuff.Identity.Api.Infrastructure.EntityFramework.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Name           = Role.Admin.Name,
                NormalizedName = Role.Admin.NormalizedName
            },
            new IdentityRole
            {
                Name           = Role.Member.Name,
                NormalizedName = Role.Member.NormalizedName
            },
            new IdentityRole
            {
                Name           = Role.RegularUser.Name,
                NormalizedName = Role.RegularUser.NormalizedName
            }
        );
    }
}
