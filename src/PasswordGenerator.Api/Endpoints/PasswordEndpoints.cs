using PasswordGenerator.Api.Domain.Dtos;
using PasswordGenerator.Api.Interfaces;

namespace PasswordGenerator.Api.Endpoints;

public static class PasswordEndpoints
{
    public static IEndpointRouteBuilder MapPasswordEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/passwords", async (IPasswordService service, CancellationToken cancellationToken) =>
        {
            var password = await service.GenerateAsync(cancellationToken);

            return Results.Created(
                $"/passwords/{password.Id}",
                new PasswordResponse(password.Id, password.Value));
        });

        app.MapGet("/passwords/{id}", async (Guid id, IPasswordService service, CancellationToken cancellationToken) =>
        {
            var password = await service.GetAsync(id, cancellationToken);

            return password is null
                ? Results.Problem(
                    title: "Senha não encontrada",
                    detail: $"Nenhuma senha foi encontrada para o identificador '{id}'.",
                    statusCode: StatusCodes.Status404NotFound)
                : Results.Ok(new PasswordResponse(password.Id, password.Value));
        });

        return app;
    }
}
