using System.Data;
using Dapper;

namespace Customers.Api.Database;

public class DatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using IDbConnection connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(
            @"CREATE TABLE IF NOT EXISTS Customers (
        Id UUID PRIMARY KEY, 
        GitHubUsername TEXT NOT NULL,
        FullName TEXT NOT NULL,
        Email TEXT NOT NULL,
        DateOfBirth TEXT NOT NULL)"
        );
    }
}