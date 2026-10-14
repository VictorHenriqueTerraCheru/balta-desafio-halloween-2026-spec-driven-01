using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api.Domain.Entities;
using PasswordGenerator.Api.Interfaces;

namespace PasswordGenerator.Api.Infrastructure;

public class PasswordRepository(PasswordDbContext context) : IPasswordRepository
{
    public async Task AddAsync(Password password, CancellationToken cancellationToken = default)
    {
        await context.Passwords.AddAsync(password, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Password?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Passwords
            .AsNoTracking()
            .FirstOrDefaultAsync(password => password.Id == id, cancellationToken);
    }
}
