namespace Identity.Infrastructure;

using Application.Interfaces;
using Broker.Configurations;
using Domain.Enum;
using KafkaFlow;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.EntityFramework;
using Security;
using Security.JwtToken;
using Shared.Application.Security;
using Shared.Infrastructure.Broker;
using Shared.Infrastructure.Broker.Configurations;
using Shared.Infrastructure.Security;
using Shared.Infrastructure.Security.CurrentUserProvider;
using Shared.Infrastructure.Security.PolicyEnforcer;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddDbContext(configuration)
            .AddAuthorization()
            .AddAuthentication(configuration)
            .AddIdentity()
            .AddBroker(configuration);

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContext<IdentityDbContext>(
            options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );

        return services;
    }

    private static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        services.AddSingleton<IPolicyEnforcer, PolicyEnforcer>();

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtTokenConfigurations>(configuration.GetSection(JwtTokenConfigurations.Section));

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services
            .ConfigureOptions<JwtTokenValidation>()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        return services;
    }

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services
            .AddIdentityCore<IdentityUser>(
                options =>
                {
                    options.Password.RequireDigit           = true;
                    options.Password.RequireLowercase       = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase       = true;
                    options.Password.RequiredLength         = 8;

                    options.User.RequireUniqueEmail = true;

                    options.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(30);
                    options.Lockout.MaxFailedAccessAttempts = 5;

                    options.SignIn.RequireConfirmedEmail = true;
                }
            )
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<TokenProvider<IdentityUser>>(TokenProvider.Chicly.Name);

        services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(2));

        return services;
    }

    private static void AddBroker(this IServiceCollection services, IConfiguration configuration)
    {
        var kafkaSection = configuration.GetSection(KafkaClientConfigurations.Section);
        services.Configure<KafkaClientConfigurations>(kafkaSection);
        var kafkaClientConfigurations = kafkaSection.Get<KafkaClientConfigurations>()!;

        services.AddSingleton<IMessageBroker, KafkaMessageBroker>();

        services.AddKafka(
            kafka => kafka
                .UseConsoleLog()
                .AddCluster(
                    cluster => cluster
                        .WithBrokers(kafkaClientConfigurations.BootstrapServers)
                        .CreateTopicIfNotExists(kafkaClientConfigurations.TopicName, 1, 1)
                        .AddProducer(
                            kafkaClientConfigurations.ProducerName,
                            producer => producer
                                .DefaultTopic(kafkaClientConfigurations.TopicName)
                                .AddMiddlewares(
                                    middlewares => middlewares.AddSerializer<SnakeCaseSerializer, ContentTypeResolver>()
                                )
                        )
                )
        );
    }
}
