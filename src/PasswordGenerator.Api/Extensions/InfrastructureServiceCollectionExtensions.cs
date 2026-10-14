using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api.Infrastructure;
using PasswordGenerator.Api.Interfaces;

namespace PasswordGenerator.Api.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
    private const string DefaultConnectionString = "Data Source=passwords.db";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Passwords") ?? DefaultConnectionString;

        services.AddDbContext<PasswordDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<IPasswordRepository, PasswordRepository>();
        services.AddSingleton<IPasswordGenerator, RandomPasswordGenerator>();
        services.AddSingleton<DatabaseInitializer>();

        return services;
    }
}
