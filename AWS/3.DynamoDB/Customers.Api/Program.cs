using Amazon.DynamoDBv2;
using Customers.Api.Repositories;
using Customers.Api.Services;
using Customers.Api.Validation;
using FluentValidation.AspNetCore;
using Microsoft.Net.Http.Headers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(
    new WebApplicationOptions { Args = args, ContentRootPath = Directory.GetCurrentDirectory() }
);

ConfigurationManager config = builder.Configuration;
config.AddEnvironmentVariables("CustomersApi_");

builder.Services.AddControllers()
       .AddFluentValidation(
           x =>
           {
               x.RegisterValidatorsFromAssemblyContaining<Program>();
               x.DisableDataAnnotationsValidation = true;
           }
       );
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddSingleton<IGitHubService, GitHubService>();

builder.Services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();

builder.Services.AddHttpClient(
    "GitHub",
    httpClient =>
    {
        httpClient.BaseAddress = new Uri(config.GetValue<string>("GitHub:ApiBaseUrl")!);
        httpClient.DefaultRequestHeaders.Add(
            HeaderNames.Accept,
            "application/vnd.github.v3+json"
        );
        httpClient.DefaultRequestHeaders.Add(
            HeaderNames.UserAgent,
            $"Course-{Environment.MachineName}"
        );
    }
);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ValidationExceptionMiddleware>();
app.MapControllers();

app.Run();