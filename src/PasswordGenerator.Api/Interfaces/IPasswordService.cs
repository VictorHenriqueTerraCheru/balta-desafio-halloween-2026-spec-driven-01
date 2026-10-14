using PasswordGenerator.Api.Domain.Entities;

namespace PasswordGenerator.Api.Interfaces;

public interface IPasswordService
{
    Task<Password> GenerateAsync(CancellationToken cancellationToken = default);

    Task<Password?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
