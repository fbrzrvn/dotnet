using System.Net;
using System.Text.Json.Nodes;

namespace Customers.Api.Services;

public class GitHubService : IGitHubService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GitHubService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> IsValidGitHubUser(string username)
    {
        HttpClient          client   = _httpClientFactory.CreateClient("GitHub");
        HttpResponseMessage response = await client.GetAsync($"/users/{username}");

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            JsonObject? responseBody = await response.Content.ReadFromJsonAsync<JsonObject>();
            string      message      = responseBody!["message"]!.ToString();
            throw new HttpRequestException(message);
        }

        return response.StatusCode == HttpStatusCode.OK;
    }
}