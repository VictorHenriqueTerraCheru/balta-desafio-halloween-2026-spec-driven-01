using PasswordGenerator.Api.Infrastructure;

namespace PasswordGenerator.Api.Extensions;

public static class DatabaseWebApplicationExtensions
{
    public static async Task<WebApplication> UseDatabaseInitialization(this WebApplication app)
    {
        var initializer = app.Services.GetRequiredService<DatabaseInitializer>();
        await initializer.EnsureCreatedAsync();

        return app;
    }
}
