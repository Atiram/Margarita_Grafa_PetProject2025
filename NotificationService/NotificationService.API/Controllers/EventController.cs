using Microsoft.AspNetCore.Mvc;
using NotificationService.BLL.Services.Interfaces;
using NotificationService.DAL.Entities;

namespace NotificationService.API.Controllers;
[ApiController]
[Route("[controller]")]
public class EventController(IEventService eventService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<EventEntity> GetEventById(Guid id)
    {
        return await eventService.GetByIdAsync(id);
    }

    [HttpGet]
    public async Task<IEnumerable<EventEntity>?> GetAll()
    {
        return await eventService.GetEventsAsync();
    }

    [HttpPost]
    public async Task<EventEntity> Post(EventEntity eventEntity)
    {
        await eventService.CreateAsync(eventEntity);
        return eventEntity;
    }

    [HttpPut]
    public async Task<EventEntity?> Put(EventEntity eventEntity)
    {
        return await eventService.UpdateAsync(eventEntity);
    }

    [HttpDelete]
    public async Task Delete(Guid id)
    {
        await eventService.DeleteAsync(id);
    }
}
