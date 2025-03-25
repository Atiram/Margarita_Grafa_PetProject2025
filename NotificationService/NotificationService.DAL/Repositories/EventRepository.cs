using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NotificationService.DAL.Entities;
using NotificationService.DAL.Repositories.Interfaces;

namespace NotificationService.DAL.Repositories;
public class EventRepository : IEventRepository
{
    private string? connectionString;
    public EventRepository(IConfiguration configuration)
    {
        this.connectionString = configuration.GetConnectionString("DBConnection") ?? throw new ArgumentException("Connection string 'DBConnection' is missing or empty in configuration.");
    }
    public async Task<EventEntity?> GetByIdAsync(Guid id)
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            IEnumerable<EventEntity> result = await db.QueryAsync<EventEntity>("SELECT * FROM Events WHERE Id = @id", new { id });
            return result.FirstOrDefault();
        }
    }

    public async Task<List<EventEntity>?> GetEventsAsync()
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            var result = await db.QueryAsync<EventEntity>("SELECT * FROM Events");
            return result.ToList();
        }
    }

    public async Task<EventEntity?> CreateAsync(EventEntity eventEntity)
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            var sqlQuery =
                "INSERT INTO Events (Type, Metadata, CreatedAt, UpdatedAt) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Type, @Metadata, @CreatedAt, @UpdatedAt);";
            eventEntity.Id = await db.QuerySingleAsync<Guid>(sqlQuery, eventEntity);
            return eventEntity;
        }
    }

    public async Task<EventEntity?> UpdateAsync(EventEntity eventEntity)
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            var sqlQuery = "UPDATE Events SET Type = @Type, Metadata = @Metadata WHERE Id = @Id";
            await db.ExecuteAsync(sqlQuery, eventEntity);
            return eventEntity;
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
