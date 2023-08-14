using Customers.Api.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Testcontainers.PostgreSql;

namespace Customers.Api.Tests.Integration;

public class CustomerApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    public const string ValidGithubUser = "validuser";
    public const string ThrottledUser = "throttle";

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithDatabase("testdb")
        .WithUsername("lerelle")
        .WithPassword("password")
        .Build();

    private readonly GitHubApiServer _gitHubApiServer = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging => logging.ClearProviders());
        builder.ConfigureTestServices(services => {
            services.RemoveAll(typeof(IDbConnectionFactory));
            services.TryAddSingleton<IDbConnectionFactory>(_ =>
                new NpgsqlConnectionFactory(_dbContainer.GetConnectionString())
            );

            services.AddHttpClient("GitHub", httpClient =>
            {
                httpClient.BaseAddress = new Uri(_gitHubApiServer.Url);
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
                httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, $"Course-{Environment.MachineName}");
            });
        });
    }

    public async Task InitializeAsync()
    {
        _gitHubApiServer.Start();
        _gitHubApiServer.SetupUser(ValidGithubUser);
        _gitHubApiServer.SetupThrottledUser(ThrottledUser);

        await _dbContainer.StartAsync();
    }

     public  new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();

        _gitHubApiServer.Dispose();
    }
}
