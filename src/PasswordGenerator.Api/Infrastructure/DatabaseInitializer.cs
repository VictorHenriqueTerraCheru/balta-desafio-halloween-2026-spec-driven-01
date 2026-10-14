using Microsoft.Extensions.DependencyInjection;

namespace PasswordGenerator.Api.Infrastructure;

public class DatabaseInitializer(IServiceScopeFactory scopeFactory)
{
    public async Task EnsureCreatedAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = scopeFactory.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<PasswordDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);
    }
}
