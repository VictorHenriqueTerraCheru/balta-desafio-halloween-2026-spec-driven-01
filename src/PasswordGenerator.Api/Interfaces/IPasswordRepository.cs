using PasswordGenerator.Api.Domain.Entities;

namespace PasswordGenerator.Api.Interfaces;

public interface IPasswordRepository
{
    Task AddAsync(Password password, CancellationToken cancellationToken = default);

    Task<Password?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
