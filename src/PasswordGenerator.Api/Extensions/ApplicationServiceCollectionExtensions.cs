using PasswordGenerator.Api.Application;
using PasswordGenerator.Api.Interfaces;

namespace PasswordGenerator.Api.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddScoped<IPasswordService, PasswordService>();

        return services;
    }
}
