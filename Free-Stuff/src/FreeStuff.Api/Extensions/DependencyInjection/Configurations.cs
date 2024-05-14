using FreeStuff.Shared.Infrastructure.MessageBroker;
using Microsoft.Extensions.Options;

namespace FreeStuff.Api.Extensions.DependencyInjection;

public static class Configurations
{
    public static IServiceCollection ConfigureApplicationServices(
        this IServiceCollection services,
        IConfiguration          configuration
    )
    {
        services.ConfigureRabbitMQ(configuration);

        return services;
    }

    private static IServiceCollection ConfigureRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMQConfig>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton(
            serviceProvider => serviceProvider.GetRequiredService<IOptions<RabbitMQConfig>>().Value
        );

        return services;
    }
}
