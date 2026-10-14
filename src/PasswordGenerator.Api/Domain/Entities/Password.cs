namespace PasswordGenerator.Api.Domain.Entities;

public class Password
{
    private Password(Guid id, string value)
    {
        Id = id;
        Value = value;
    }

    public Guid Id { get; }

    public string Value { get; }

    public static Password Create(string value) => new(Guid.NewGuid(), value);
}
