using Microsoft.AspNetCore.Mvc;
using NotificationService.DAL.Entities;
using NotificationService.DAL.Repositories.Interfaces;

namespace NotificationService.API.Controllers;
[ApiController]
[Route("[controller]")]
public class EventController(IEventRepository eventRepository) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<EventEntity> GetEventById(Guid id)
    {
        return await eventRepository.GetByIdAsync(id);
    }

    [HttpGet]
    public async Task<IEnumerable<EventEntity>> GetAll()
    {
        return await eventRepository.GetEventsAsync();
    }

    [HttpPost]
    public async Task<EventEntity> Post(EventEntity eventEntity)
    {
        await eventRepository.CreateAsync(eventEntity);
        return eventEntity;
    }

    [HttpPut]
    public async Task Put(EventEntity eventEntity)
    {
        await eventRepository.UpdateAsync(eventEntity);
    }

    [HttpDelete]
    public async Task Delete(Guid id)
    {
        await eventRepository.DeleteAsync(id);
    }
}
