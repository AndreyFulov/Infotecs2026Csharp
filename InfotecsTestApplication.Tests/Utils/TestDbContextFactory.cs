using InfotecsTestApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace InfotecsTestApplication.Tests;

public class TestDbContextFactory : IDisposable
{
    public AppDbContext Context { get; }

    private readonly string _databaseName;

    public TestDbContextFactory()
    {
        _databaseName = Guid.NewGuid().ToString();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(_databaseName)
            .ConfigureWarnings(warnings =>
            {
                warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning);
            })
            .Options;

        Context = new AppDbContext(options);
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}