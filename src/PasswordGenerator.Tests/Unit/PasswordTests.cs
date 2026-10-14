using PasswordGenerator.Api.Domain.Entities;

namespace PasswordGenerator.Tests.Unit;

public class PasswordTests
{
    [Fact]
    public void Create_GeneratesNonEmptyId()
    {
        var password = Password.Create("aB3$kL9#mQ2!xZ7&pR4@");

        Assert.NotEqual(Guid.Empty, password.Id);
    }

    [Fact]
    public void Create_GeneratesDistinctIdsForEachCall()
    {
        var first = Password.Create("aB3$kL9#mQ2!xZ7&pR4@");
        var second = Password.Create("aB3$kL9#mQ2!xZ7&pR4@");

        Assert.NotEqual(first.Id, second.Id);
    }

    [Fact]
    public void Create_PreservesValue()
    {
        const string value = "aB3$kL9#mQ2!xZ7&pR4@";

        var password = Password.Create(value);

        Assert.Equal(value, password.Value);
    }
}
