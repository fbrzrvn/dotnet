using FreeStuff.Api;
using FreeStuff.Api.Extensions.DependencyInjection;
using FreeStuff.Shared.Infrastructure.EntityFramework;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog(
        (context, cfg) =>
        {
            cfg.ReadFrom.Configuration(context.Configuration);
        }
    );

    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();
    builder.Services.ConfigureApplicationServices(builder.Configuration);

    builder.Services.AddHealthChecks()
           .AddMySql(builder.Configuration.GetConnectionString("Default")!);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();

    app.UseExceptionHandler(ApiEndpoints.Errors);

    app.UseHttpsRedirection();

    app.MapHealthChecks(ApiEndpoints.Health, new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }
    );

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<FreeStuffDbContext>();
        dbContext.Database.Migrate();
    }

    app.Run();
}
