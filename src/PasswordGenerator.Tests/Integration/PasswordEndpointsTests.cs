using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordGenerator.Api.Domain.Dtos;
using PasswordGenerator.Api.Domain.Entities;

namespace PasswordGenerator.Tests.Integration;

public class PasswordEndpointsTests : IClassFixture<PasswordApiFactory>
{
    private readonly HttpClient _client;

    public PasswordEndpointsTests(PasswordApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostPasswords_ReturnsCreatedWithIdAndPassword()
    {
        var response = await _client.PostAsync("/passwords", content: null);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<PasswordResponse>();

        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.Id);
        Assert.Equal(PasswordPattern.Length, body.Password.Length);
        AssertFollowsCompositionRules(body.Password);
    }

    [Fact]
    public async Task GetPasswordById_ReturnsOkWithSamePasswordEveryTime()
    {
        var created = await _client.PostAsync("/passwords", content: null);
        var generated = await created.Content.ReadFromJsonAsync<PasswordResponse>();

        Assert.NotNull(generated);

        var first = await _client.GetAsync($"/passwords/{generated.Id}");
        var second = await _client.GetAsync($"/passwords/{generated.Id}");

        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        Assert.Equal(HttpStatusCode.OK, second.StatusCode);

        var firstBody = await first.Content.ReadFromJsonAsync<PasswordResponse>();
        var secondBody = await second.Content.ReadFromJsonAsync<PasswordResponse>();

        Assert.Equal(generated.Password, firstBody?.Password);
        Assert.Equal(generated.Password, secondBody?.Password);
    }

    [Fact]
    public async Task GetPasswordById_WithUppercaseId_ReturnsOk()
    {
        var created = await _client.PostAsync("/passwords", content: null);
        var generated = await created.Content.ReadFromJsonAsync<PasswordResponse>();

        Assert.NotNull(generated);

        var uppercaseId = generated.Id.ToString().ToUpperInvariant();
        var response = await _client.GetAsync($"/passwords/{uppercaseId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetPasswordById_WithUnknownId_ReturnsNotFoundProblemDetails()
    {
        var response = await _client.GetAsync($"/passwords/{Guid.NewGuid()}");

        await AssertNotFoundProblemDetailsAsync(response);
    }

    [Fact]
    public async Task GetPasswordById_WithZeroId_ReturnsNotFoundProblemDetails()
    {
        var response = await _client.GetAsync($"/passwords/{Guid.Empty}");

        await AssertNotFoundProblemDetailsAsync(response);
    }

    [Fact]
    public async Task GetPasswordById_WithMalformedId_ReturnsBadRequestProblemDetails()
    {
        var response = await _client.GetAsync("/passwords/abc");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(StatusCodes.Status400BadRequest, problem.Status);
    }

    private static async Task AssertNotFoundProblemDetailsAsync(HttpResponseMessage response)
    {
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(StatusCodes.Status404NotFound, problem.Status);
    }

    private static void AssertFollowsCompositionRules(string password)
    {
        Assert.Contains(password, char.IsUpper);
        Assert.Contains(password, char.IsLower);
        Assert.Contains(password, char.IsDigit);
        Assert.Contains(password, c => PasswordPattern.Special.Contains(c));
        Assert.All(password, c => Assert.Contains(c, PasswordPattern.All));
        Assert.DoesNotContain(password, char.IsWhiteSpace);
    }
}
