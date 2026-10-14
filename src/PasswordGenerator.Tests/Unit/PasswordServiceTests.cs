using PasswordGenerator.Api.Application;
using PasswordGenerator.Api.Domain.Entities;
using PasswordGenerator.Api.Interfaces;

namespace PasswordGenerator.Tests.Unit;

public class PasswordServiceTests
{
    private const string GeneratedValue = "aB3$kL9#mQ2!xZ7&pR4@";

    [Fact]
    public async Task GenerateAsync_PersistsGeneratedPassword()
    {
        var generator = new FakePasswordGenerator(GeneratedValue);
        var repository = new FakePasswordRepository();
        var service = new PasswordService(generator, repository);

        var password = await service.GenerateAsync();

        var saved = Assert.Single(repository.Saved);
        Assert.Same(password, saved);
    }

    [Fact]
    public async Task GenerateAsync_ReturnsIdAndValue()
    {
        var generator = new FakePasswordGenerator(GeneratedValue);
        var repository = new FakePasswordRepository();
        var service = new PasswordService(generator, repository);

        var password = await service.GenerateAsync();

        Assert.NotEqual(Guid.Empty, password.Id);
        Assert.Equal(GeneratedValue, password.Value);
    }

    [Fact]
    public async Task GetAsync_ExistingId_ReturnsPassword()
    {
        var generator = new FakePasswordGenerator(GeneratedValue);
        var repository = new FakePasswordRepository();
        var service = new PasswordService(generator, repository);
        var generated = await service.GenerateAsync();

        var found = await service.GetAsync(generated.Id);

        Assert.NotNull(found);
        Assert.Equal(generated.Id, found.Id);
        Assert.Equal(GeneratedValue, found.Value);
    }

    [Fact]
    public async Task GetAsync_UnknownId_ReturnsNull()
    {
        var service = new PasswordService(new FakePasswordGenerator(GeneratedValue), new FakePasswordRepository());

        var found = await service.GetAsync(Guid.NewGuid());

        Assert.Null(found);
    }

    [Fact]
    public async Task GetAsync_CalledTwice_ReturnsSameValue()
    {
        var service = new PasswordService(new FakePasswordGenerator(GeneratedValue), new FakePasswordRepository());
        var generated = await service.GenerateAsync();

        var first = await service.GetAsync(generated.Id);
        var second = await service.GetAsync(generated.Id);

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.Equal(first.Value, second.Value);
    }

    private sealed class FakePasswordGenerator(string value) : IPasswordGenerator
    {
        public string Generate() => value;
    }

    private sealed class FakePasswordRepository : IPasswordRepository
    {
        public List<Password> Saved { get; } = [];

        public Task AddAsync(Password password, CancellationToken cancellationToken = default)
        {
            Saved.Add(password);
            return Task.CompletedTask;
        }

        public Task<Password?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Saved.FirstOrDefault(password => password.Id == id));
        }
    }
}
