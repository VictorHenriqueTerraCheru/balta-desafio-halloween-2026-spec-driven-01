using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api.Infrastructure;

namespace PasswordGenerator.Tests.Integration;

public class PasswordDbContextTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly PasswordDbContext _context;

    public PasswordDbContextTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<PasswordDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new PasswordDbContext(options);
        _context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public void EnsureCreated_CreatesPasswordsTableWithIdAsPrimaryKey()
    {
        var columns = ReadColumns("Passwords");

        Assert.True(columns.ContainsKey("Id"));
        Assert.True(columns["Id"].IsPrimaryKey);
    }

    [Fact]
    public void EnsureCreated_MakesValueRequired()
    {
        var columns = ReadColumns("Passwords");

        Assert.True(columns.ContainsKey("Value"));
        Assert.True(columns["Value"].IsNotNull);
    }

    private Dictionary<string, (bool IsPrimaryKey, bool IsNotNull)> ReadColumns(string table)
    {
        var columns = new Dictionary<string, (bool IsPrimaryKey, bool IsNotNull)>();

        using var command = _connection.CreateCommand();
        command.CommandText = $"PRAGMA table_info('{table}');";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var name = reader.GetString(1);
            columns[name] = (IsPrimaryKey: reader.GetInt32(5) > 0, IsNotNull: reader.GetInt32(3) == 1);
        }

        return columns;
    }
}
