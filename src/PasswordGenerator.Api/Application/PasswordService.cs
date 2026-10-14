using PasswordGenerator.Api.Domain.Entities;
using PasswordGenerator.Api.Interfaces;

namespace PasswordGenerator.Api.Application;

public class PasswordService(IPasswordGenerator generator, IPasswordRepository repository) : IPasswordService
{
    public async Task<Password> GenerateAsync(CancellationToken cancellationToken = default)
    {
        var value = generator.Generate();
        var password = Password.Create(value);

        await repository.AddAsync(password, cancellationToken);

        return password;
    }

    public Task<Password?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return repository.GetByIdAsync(id, cancellationToken);
    }
}
