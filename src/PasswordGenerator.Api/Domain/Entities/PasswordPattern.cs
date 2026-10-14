namespace PasswordGenerator.Api.Domain.Entities;

public static class PasswordPattern
{
    public const int Length = 20;

    public const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public const string Lowercase = "abcdefghijklmnopqrstuvwxyz";

    public const string Digits = "0123456789";

    public const string Special = "!@#$%^&*()-_=+[]{}?.";

    public static readonly string All = Uppercase + Lowercase + Digits + Special;
}
