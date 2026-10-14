using System.Security.Cryptography;
using PasswordGenerator.Api.Domain.Entities;
using PasswordGenerator.Api.Interfaces;

namespace PasswordGenerator.Api.Infrastructure;

public class RandomPasswordGenerator : IPasswordGenerator
{
    private static readonly string[] Categories =
    [
        PasswordPattern.Uppercase,
        PasswordPattern.Lowercase,
        PasswordPattern.Digits,
        PasswordPattern.Special
    ];

    public string Generate()
    {
        var chars = new char[PasswordPattern.Length];

        for (var i = 0; i < Categories.Length; i++)
        {
            chars[i] = PickFrom(Categories[i]);
        }

        for (var i = Categories.Length; i < chars.Length; i++)
        {
            chars[i] = PickFrom(PasswordPattern.All);
        }

        Shuffle(chars);

        return new string(chars);
    }

    private static char PickFrom(string alphabet) =>
        alphabet[RandomNumberGenerator.GetInt32(alphabet.Length)];

    private static void Shuffle(char[] chars)
    {
        for (var i = chars.Length - 1; i > 0; i--)
        {
            var j = RandomNumberGenerator.GetInt32(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }
    }
}
