using System.Net;

namespace PasswordGenerator.Tests.Integration;

public class SwaggerTests : IClassFixture<PasswordApiFactory>
{
    private readonly HttpClient _client;

    public SwaggerTests(PasswordApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task SwaggerUi_ReturnsOk()
    {
        var response = await _client.GetAsync("/swagger/index.html");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task OpenApiDocument_ListsPasswordEndpoints()
    {
        var response = await _client.GetAsync("/openapi/v1.json");
        var document = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("\"/passwords\"", document);
        Assert.Contains("\"/passwords/{id}\"", document);
    }
}
