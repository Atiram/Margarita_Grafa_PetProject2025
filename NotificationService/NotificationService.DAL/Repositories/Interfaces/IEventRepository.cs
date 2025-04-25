using NotificationService.DAL.Entities;

namespace NotificationService.DAL.Repositories.Interfaces;
public interface IEventRepository
{
    Task<EventEntity?> GetByIdAsync(Guid id);
    Task<List<EventEntity>?> GetAllAsync();
    Task<EventEntity> CreateAsync(EventEntity eventEntity);
    Task<EventEntity> UpdateAsync(EventEntity eventEntity);
    Task DeleteAsync(Guid id);
}
