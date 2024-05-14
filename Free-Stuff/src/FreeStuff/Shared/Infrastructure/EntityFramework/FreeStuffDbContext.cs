using FreeStuff.Categories.Domain;
using FreeStuff.Categories.Domain.ValueObjects;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.ValueObjects;
using FreeStuff.User.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FreeStuff.Shared.Infrastructure.EntityFramework;

public class FreeStuffDbContext : DbContext
{
    public FreeStuffDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Item>     Items      { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FreeStuffDbContext).Assembly);

        modelBuilder.Entity<Item>(
            entity =>
            {
                entity.HasKey(item => item.Id);
                entity.Property(item => item.Id).HasConversion(id => id.Value, value => ItemId.Create(value));

                entity.Property(item => item.Title).HasMaxLength(100);

                entity.Property(item => item.Description).HasMaxLength(500);

                entity.HasOne(item => item.Category)
                      .WithMany(category => category.Items)
                      .HasForeignKey(item => item.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(item => item.UserId)
                      .HasConversion(userId => userId.Value, value => UserId.Create(value));
            }
        );

        modelBuilder.Entity<Category>(
            entity =>
            {
                entity.HasKey(category => category.Id);
                entity.Property(category => category.Id)
                      .HasConversion(id => id.Value, value => CategoryId.Create(value));

                entity.HasIndex(category => category.Name).IsUnique();
                entity.Property(category => category.Name).HasMaxLength(100);

                entity.Property(category => category.Description).HasMaxLength(500);

                entity.HasMany(category => category.Items)
                      .WithOne(item => item.Category)
                      .HasForeignKey(item => item.CategoryId);
            }
        );

        // Setting to avoid generating primary key for all entities
        modelBuilder.Model.GetEntityTypes()
                    .SelectMany(e => e.GetProperties())
                    .Where(p => p.IsPrimaryKey())
                    .ToList()
                    .ForEach(p => p.ValueGenerated = ValueGenerated.Never);

        base.OnModelCreating(modelBuilder);
    }
}
