using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NotificationService.DAL.Entities;
using NotificationService.DAL.Repositories.Interfaces;

namespace NotificationService.DAL.Repositories;
public class EventRepository : IEventRepository
{
    string? connectionString = null;
    public EventRepository(string conn)
    {
        connectionString = conn;
    }
    public async Task<EventEntity?> GetByIdAsync(Guid id)
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            IEnumerable<EventEntity> result = await db.QueryAsync<EventEntity>("SELECT * FROM Events WHERE Id = @id", new { id });
            return result.FirstOrDefault();
        }
    }

    public async Task<List<EventEntity>> GetEventsAsync()
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            var result = await db.QueryAsync<EventEntity>("SELECT * FROM Events");
            return result.ToList();
        }
    }

    public async Task CreateAsync(EventEntity eventEntity)
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            var sqlQuery =
                "INSERT INTO Events (Id, Type, CreatedAt, UpdatedAt) VALUES(@Id, @Type, @CreatedAt, @UpdatedAt)";
            await db.ExecuteAsync(sqlQuery, eventEntity);
        }
    }

    public async Task UpdateAsync(EventEntity eventEntity)
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            var sqlQuery = "UPDATE Events SET Type = @Type WHERE Id = @Id";
            await db.ExecuteAsync(sqlQuery, eventEntity);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            var sqlQuery = "DELETE FROM Events WHERE Id = @id";
            await db.ExecuteAsync(sqlQuery, new { id });
        }
    }
}
