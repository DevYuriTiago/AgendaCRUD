using System.Data;
using ContactAgenda.Application.DTOs;
using ContactAgenda.Application.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ContactAgenda.Infrastructure.Repositories;

/// <summary>
/// Dapper repository for optimized read operations
/// </summary>
public class DapperContactReadRepository : IContactReadRepository
{
    private readonly string _connectionString;

    public DapperContactReadRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<ContactDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT 
                Id, 
                Name, 
                Email, 
                Phone, 
                CreatedAt, 
                UpdatedAt
            FROM Contacts
            WHERE Id = @Id";

        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ContactDto>(sql, new { Id = id });
    }

    public async Task<IReadOnlyList<ContactListDto>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT 
                Id, 
                Name, 
                Email, 
                Phone, 
                CreatedAt
            FROM Contacts
            ORDER BY Name";

        using var connection = CreateConnection();
        var result = await connection.QueryAsync<ContactListDto>(sql);
        return result.ToList();
    }
}
