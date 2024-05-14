using MassTransit;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddMassTransit(
        busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            busConfigurator.SetInMemorySagaRepositoryProvider();

            var assembly = typeof(Program).Assembly;

            busConfigurator.AddConsumers(assembly);
            busConfigurator.AddSagaStateMachines(assembly);
            busConfigurator.AddSagas(assembly);
            busConfigurator.AddActivities(assembly);

            busConfigurator.UsingRabbitMq(
                (context, cfg) =>
                {
                    cfg.Host("localhost", "/",
                        c =>
                        {
                            c.Username("guest");
                            c.Password("guest");
                        }
                    );

                    cfg.ConfigureEndpoints(context);
                }
            );
        }
    );
}

var app = builder.Build();
{
    app.Run();
}
