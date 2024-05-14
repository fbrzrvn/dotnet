using FreeStuff.Categories.Domain;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Enum;
using FreeStuff.Shared.Infrastructure.EntityFramework;
using FreeStuff.Tests.Utils.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MySql;

namespace FreeStuff.Api.Tests.Integration;

public class FreeStuffApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly MySqlContainer _dbContainer = new MySqlBuilder()
                                                   .WithImage("mysql:latest")
                                                   .WithDatabase("fs-db-test")
                                                   .WithUsername("root")
                                                   .WithPassword("password")
                                                   .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(
            services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<FreeStuffDbContext>)
                );

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var connectionString = _dbContainer.GetConnectionString();
                services.AddDbContext<FreeStuffDbContext>(
                    options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                );
            }
        );
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await EnsureDatabaseCreated();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    private async Task EnsureDatabaseCreated()
    {
        using var scope     = Services.CreateScope();
        var       dbContext = scope.ServiceProvider.GetRequiredService<FreeStuffDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
        await SeedData(dbContext);
    }

    private static async Task SeedData(FreeStuffDbContext context)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var category = Category.Create(Constants.Category.Test, Constants.Category.Description);
            context.Categories.Add(category);

            if (!context.Items.Any())
            {
                context.Items.AddRange(
                    Item.Create(
                        "item A",
                        Constants.Item.Description,
                        category,
                        ItemCondition.New,
                        Constants.Item.UserId
                    ),
                    Item.Create(
                        "item B",
                        Constants.Item.Description,
                        category,
                        ItemCondition.HasGivenItAll,
                        Constants.Item.UserId
                    ),
                    Item.Create(
                        "item C",
                        Constants.Item.Description,
                        category,
                        ItemCondition.GoodCondition,
                        Constants.Item.UserId
                    ),
                    Item.Create(
                        "item D",
                        Constants.Item.Description,
                        category,
                        ItemCondition.FairCondition,
                        Constants.Item.UserId
                    )
                );
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
