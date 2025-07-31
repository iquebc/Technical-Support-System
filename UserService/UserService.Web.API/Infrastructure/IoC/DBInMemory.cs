using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UserService.Web.API.Infrastructure.Context;


namespace UserService.Web.API.Infrastructure.IoC;

public class DBInMemory
{
    private ApplicationDbContext _context;

    private SqliteConnection _connection;

    public DBInMemory(ILoggerFactory loggerFactory)
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection).UseLoggerFactory(loggerFactory).UseLazyLoadingProxies().EnableSensitiveDataLogging(true).Options;

        _context = new ApplicationDbContext(options);

        _context.Database.EnsureCreated();
    }

    public DBInMemory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection).UseLazyLoadingProxies().EnableSensitiveDataLogging(true).Options;

        _context = new ApplicationDbContext(options);

        _context.Database.EnsureCreated();
    }

    public ApplicationDbContext GetContext() => _context;

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}
