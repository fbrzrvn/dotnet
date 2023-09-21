using Amazon.SQS;
using Customers.Consumer;
using MediatR;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager config = builder.Configuration;

builder.Services.Configure<QueueSettings>(config.GetSection(QueueSettings.Key));
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
builder.Services.AddHostedService<QueueConsumerService>();
builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly)
);

WebApplication app = builder.Build();

app.Run();