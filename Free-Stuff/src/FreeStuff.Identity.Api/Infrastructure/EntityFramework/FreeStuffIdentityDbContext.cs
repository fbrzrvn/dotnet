using FreeStuff.Identity.Api.Domain;
using FreeStuff.Identity.Api.Infrastructure.EntityFramework.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreeStuff.Identity.Api.Infrastructure.EntityFramework;

public class FreeStuffIdentityDbContext : IdentityDbContext<User>
{
    public FreeStuffIdentityDbContext(DbContextOptions<FreeStuffIdentityDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Domain.Token> Tokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RoleConfiguration());

        modelBuilder.Entity<Domain.Token>(
            entity =>
            {
                entity.HasKey(token => token.Id);

                entity.HasIndex(token => token.Value).IsUnique();
                entity.Property(token => token.Value).HasMaxLength(512);
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}
