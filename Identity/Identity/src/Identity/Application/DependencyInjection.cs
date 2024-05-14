namespace Identity.Application;

using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Behaviors;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddMediatr(services);
        AddMapper(services);

        return services;
    }

    private static IServiceCollection AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(
            cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            }
        );

        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        return services;
    }

    private static IServiceCollection AddMapper(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(DependencyInjection).Assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
