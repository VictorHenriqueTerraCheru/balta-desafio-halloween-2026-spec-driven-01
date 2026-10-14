using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api.Domain.Entities;
using PasswordGenerator.Api.Infrastructure;

namespace PasswordGenerator.Tests.Integration;

public class PasswordRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<PasswordDbContext> _options;

    public PasswordRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<PasswordDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new PasswordDbContext(_options);
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    [Fact]
    public async Task AddAsync_SavedPassword_CanBeReadBackById()
    {
        var password = Password.Create("aB3$kL9#mQ2!xZ7&pR4@");

        using (var writer = new PasswordDbContext(_options))
        {
            await new PasswordRepository(writer).AddAsync(password);
        }

        using var reader = new PasswordDbContext(_options);
        var found = await new PasswordRepository(reader).GetByIdAsync(password.Id);

        Assert.NotNull(found);
        Assert.Equal(password.Id, found.Id);
        Assert.Equal(password.Value, found.Value);
    }

    [Fact]
    public async Task GetByIdAsync_UnknownId_ReturnsNull()
    {
        using var context = new PasswordDbContext(_options);
        var repository = new PasswordRepository(context);

        var found = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(found);
    }

    [Fact]
    public async Task GetByIdAsync_CalledTwice_ReturnsSameValue()
    {
        var password = Password.Create("aB3$kL9#mQ2!xZ7&pR4@");

        using (var writer = new PasswordDbContext(_options))
        {
            await new PasswordRepository(writer).AddAsync(password);
        }

        using var context = new PasswordDbContext(_options);
        var repository = new PasswordRepository(context);

        var first = await repository.GetByIdAsync(password.Id);
        var second = await repository.GetByIdAsync(password.Id);

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.Equal(first.Value, second.Value);
    }
}
