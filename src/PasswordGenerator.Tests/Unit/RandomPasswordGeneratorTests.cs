using PasswordGenerator.Api.Infrastructure;
using PasswordGenerator.Api.Interfaces;

namespace PasswordGenerator.Tests.Unit;

public class RandomPasswordGeneratorTests
{
    private const int Iterations = 1000;
    private const int ExpectedLength = 20;

    private const string AllowedSpecial = "!@#$%^&*()-_=+[]{}?.";

    [Fact]
    public void Generate_RespectsCompositionRulesInEveryGeneration()
    {
        IPasswordGenerator generator = new RandomPasswordGenerator();

        for (var i = 0; i < Iterations; i++)
        {
            var password = generator.Generate();

            Assert.Equal(ExpectedLength, password.Length);
            Assert.True(password.Any(char.IsAsciiLetterUpper), "Sem letra maiúscula");
            Assert.True(password.Any(char.IsAsciiLetterLower), "Sem letra minúscula");
            Assert.True(password.Any(char.IsAsciiDigit), "Sem número");
            Assert.True(password.Any(c => AllowedSpecial.Contains(c)), "Sem caractere especial");
            Assert.DoesNotContain(' ', password);
            Assert.All(password, c => Assert.True(
                char.IsAsciiLetterOrDigit(c) || AllowedSpecial.Contains(c),
                "Caractere fora do conjunto permitido"));
        }
    }

    [Fact]
    public void Generate_ProducesDifferentPasswordsOnConsecutiveCalls()
    {
        var generator = new RandomPasswordGenerator();

        var first = generator.Generate();
        var second = generator.Generate();

        Assert.NotEqual(first, second);
    }
}
