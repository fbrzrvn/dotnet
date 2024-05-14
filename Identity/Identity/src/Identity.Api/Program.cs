using Identity.Api;
using Identity.Application;
using Identity.Infrastructure;
using Identity.Infrastructure.Persistence.EntityFramework;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog(
        (context, cfg) =>
        {
            cfg.ReadFrom.Configuration(context.Configuration);
        }
    );

    builder.Services.AddPresentation();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment() ||
        app.Environment.EnvironmentName.Equals("docker", StringComparison.CurrentCultureIgnoreCase))
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();

    app.UseExceptionHandler(ApiEndpoints.Errors);

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        dbContext.Database.EnsureCreated();
    }

    app.Run();
}
