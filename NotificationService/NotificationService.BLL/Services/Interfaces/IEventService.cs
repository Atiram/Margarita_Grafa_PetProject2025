using Clinic.DOMAIN;
using NotificationService.DAL.Entities;

namespace NotificationService.BLL.Services.Interfaces;
public interface IEventService
{
    Task<EventEntity?> GetByIdAsync(Guid id);
    Task<List<EventEntity>?> GetEventsAsync();
    Task<EventEntity?> CreateAsync(CreateEventMail eventEntity);
    Task<EventEntity?> UpdateAsync(EventEntity eventEntity);
    Task DeleteAsync(Guid id);
}
