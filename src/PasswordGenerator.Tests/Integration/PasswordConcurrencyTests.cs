using System.Net;
using System.Net.Http.Json;
using PasswordGenerator.Api.Domain.Dtos;

namespace PasswordGenerator.Tests.Integration;

public class PasswordConcurrencyTests : IClassFixture<PasswordApiFactory>
{
    private const int ConcurrentRequests = 50;

    private readonly HttpClient _client;

    public PasswordConcurrencyTests(PasswordApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostPasswords_WithConcurrentRequests_ReturnsDistinctIdsAndPasswords()
    {
        var tasks = Enumerable.Range(0, ConcurrentRequests)
            .Select(_ => _client.PostAsync("/passwords", content: null))
            .ToArray();

        var responses = await Task.WhenAll(tasks);

        Assert.All(responses, response => Assert.Equal(HttpStatusCode.Created, response.StatusCode));

        var bodies = await Task.WhenAll(
            responses.Select(response => response.Content.ReadFromJsonAsync<PasswordResponse>()));

        Assert.All(bodies, body => Assert.NotNull(body));
        Assert.Equal(ConcurrentRequests, bodies.Select(body => body!.Id).Distinct().Count());
        Assert.Equal(ConcurrentRequests, bodies.Select(body => body!.Password).Distinct().Count());
    }
}
