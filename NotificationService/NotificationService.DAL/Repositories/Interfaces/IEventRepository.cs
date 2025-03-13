using NotificationService.DAL.Entities;

namespace NotificationService.DAL.Repositories.Interfaces;
public interface IEventRepository
{
    Task<EventEntity> GetByIdAsync(Guid id);
    Task<List<EventEntity>> GetEventsAsync();
    Task CreateAsync(EventEntity eventEntity);
    Task UpdateAsync(EventEntity eventEntity);
    Task DeleteAsync(Guid id);
}
