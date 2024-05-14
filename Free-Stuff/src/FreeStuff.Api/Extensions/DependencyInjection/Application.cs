using System.Reflection;
using FluentValidation;
using FreeStuff.Shared.Application.Behaviors;
using Mapster;
using MapsterMapper;
using MediatR;

namespace FreeStuff.Api.Extensions.DependencyInjection;

public static class Application
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(
            cfg => cfg.RegisterServicesFromAssembly(typeof(IApplicationMarker).GetTypeInfo().Assembly)
        );

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));
        services.AddValidatorsFromAssembly(typeof(IApplicationMarker).GetTypeInfo().Assembly);

        services.AddMapper();

        return services;
    }

    private static IServiceCollection AddMapper(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(IApiMarker).Assembly, typeof(IApplicationMarker).Assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
