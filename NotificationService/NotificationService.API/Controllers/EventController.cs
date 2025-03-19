using AutoMapper;
using Clinic.DOMAIN;
using Microsoft.AspNetCore.Mvc;
using NotificationService.API.ViewModels;
using NotificationService.BLL.Services.Interfaces;
using NotificationService.DAL.Entities;

namespace NotificationService.API.Controllers;
[ApiController]
[Route("[controller]")]
public class EventController(IEventService eventService, IMapper mapper) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<EventViewModel> GetEventById([FromRoute] Guid id)
    {
        var eventEntity = await eventService.GetByIdAsync(id);
        return mapper.Map<EventViewModel>(eventEntity);
    }

    [HttpGet]
    public async Task<IEnumerable<EventViewModel>> GetAll()
    {
        var eventEntities = await eventService.GetEventsAsync();
        return mapper.Map<IEnumerable<EventViewModel>>(eventEntities);
    }

    [HttpPost]
    public async Task<EventViewModel> Post(CreateEventMail request)
    {
        var createdEvent = await eventService.CreateAsync(request);
        return mapper.Map<EventViewModel>(createdEvent);
    }

    [HttpPut]
    public async Task<EventViewModel?> Put(EventEntity eventEntity)
    {
        var updatedEvent = await eventService.UpdateAsync(eventEntity);
        return mapper.Map<EventViewModel>(updatedEvent);
    }

    [HttpDelete]
    public async Task Delete(Guid id)
    {
        await eventService.DeleteAsync(id);
    }
}
