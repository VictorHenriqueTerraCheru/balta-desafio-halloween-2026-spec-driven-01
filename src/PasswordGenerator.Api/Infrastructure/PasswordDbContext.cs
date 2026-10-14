using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api.Domain.Entities;

namespace PasswordGenerator.Api.Infrastructure;

public class PasswordDbContext(DbContextOptions<PasswordDbContext> options) : DbContext(options)
{
    public DbSet<Password> Passwords => Set<Password>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PasswordConfiguration());
    }
}
