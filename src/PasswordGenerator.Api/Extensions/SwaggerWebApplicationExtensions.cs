using Swashbuckle.AspNetCore.SwaggerUI;

namespace PasswordGenerator.Api.Extensions;

public static class SwaggerWebApplicationExtensions
{
    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        app.MapOpenApi();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "v1");
            options.RoutePrefix = "swagger";
            options.DocumentTitle = "Password Generator API";
        });

        return app;
    }
}
