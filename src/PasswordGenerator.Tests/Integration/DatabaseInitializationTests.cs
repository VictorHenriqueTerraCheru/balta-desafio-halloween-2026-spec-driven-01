using System.Net;
using Microsoft.Extensions.DependencyInjection;
using PasswordGenerator.Api.Infrastructure;

namespace PasswordGenerator.Tests.Integration;

public class DatabaseInitializationTests
{
    [Fact]
    public async Task PostPasswords_OnEmptyDatabase_ReturnsCreated()
    {
        using var factory = new PasswordApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsync("/passwords", content: null);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task EnsureCreatedAsync_CalledTwice_DoesNotFail()
    {
        using var factory = new PasswordApiFactory();
        using var client = factory.CreateClient();

        var initializer = factory.Services.GetRequiredService<DatabaseInitializer>();

        await initializer.EnsureCreatedAsync();
        await initializer.EnsureCreatedAsync();
    }
}
