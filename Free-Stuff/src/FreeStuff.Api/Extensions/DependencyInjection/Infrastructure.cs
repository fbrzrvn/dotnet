using FreeStuff.Categories.Domain.Ports;
using FreeStuff.Categories.Infrastructure;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Infrastructure;
using FreeStuff.Shared.Domain;
using FreeStuff.Shared.Infrastructure.EntityFramework;
using FreeStuff.Shared.Infrastructure.MessageBroker;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace FreeStuff.Api.Extensions.DependencyInjection;

public static class Infrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        services.AddRabbitMQ();

        services.AddScoped<IItemRepository, EfItemRepository>();
        services.AddScoped<ICategoryRepository, EfCategoryRepository>();

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContext<FreeStuffDbContext>(
            options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );

        return services;
    }

    private static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        services.AddMassTransit(
            busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.UsingRabbitMq(
                    (context, cfg) =>
                    {
                        var config = context.GetRequiredService<RabbitMQConfig>();
                        cfg.Host(
                            new Uri(config.Host),
                            c =>
                            {
                                c.Username(config.Username);
                                c.Password(config.Password);
                            }
                        );

                        cfg.ConfigureEndpoints(context);
                    }
                );
            }
        );

        services.AddTransient<IEventBus, EventBus>();

        return services;
    }
}
