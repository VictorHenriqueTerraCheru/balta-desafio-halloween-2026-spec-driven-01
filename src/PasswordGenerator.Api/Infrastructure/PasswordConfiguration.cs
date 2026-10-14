using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordGenerator.Api.Domain.Entities;

namespace PasswordGenerator.Api.Infrastructure;

public class PasswordConfiguration : IEntityTypeConfiguration<Password>
{
    public void Configure(EntityTypeBuilder<Password> builder)
    {
        builder.ToTable("Passwords");

        builder.HasKey(password => password.Id);

        builder.Property(password => password.Id).ValueGeneratedNever();

        builder.Property(password => password.Value).IsRequired();
    }
}
