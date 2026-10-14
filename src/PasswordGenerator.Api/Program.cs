using PasswordGenerator.Api.Endpoints;
using PasswordGenerator.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

app.MapPasswordEndpoints();
app.UseSwaggerDocumentation();
await app.UseDatabaseInitialization();

app.Run();

public partial class Program;
