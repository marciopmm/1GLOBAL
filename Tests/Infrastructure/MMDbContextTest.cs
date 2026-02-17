using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MM.Infrastructure.Persistence.Db;
using System.Collections.Generic;
using System.IO;

namespace MM.Tests.MMDb;

[TestClass]
public class MMDbContextTest
{
    [TestMethod]
    public void CtorMMDbContext_CheckConnectionString()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"ConnectionStrings:Default", "Data Source=test/MMDb.sqlite"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Act
        using var dbContext = new MMDbContext(configuration);
        var connectionString = dbContext.Database.GetDbConnection().ConnectionString;
        var sqliteBuilder = new SqliteConnectionStringBuilder(connectionString);
        var expectedDataSource = Path.Combine(Directory.GetCurrentDirectory(), "test\\MMDb.sqlite");

        // Assert
        Assert.AreEqual(expectedDataSource, sqliteBuilder.DataSource);
    }
}
