using MediatR;
using NotificationService.BLL.Mediator.Requests;
using NotificationService.BLL.Services.Interfaces;
using NotificationService.DAL.Entities;

namespace NotificationService.BLL.Services;
public class EventService(IMediator mediator) : IEventService
{
    public async Task<EventEntity?> GetByIdAsync(Guid id)
    {
        var eventDetails = await mediator.Send(new GetEventRequest() { Id = id });
        return eventDetails;
    }

    public async Task<List<EventEntity>?> GetEventsAsync()
    {
        var eventDetails = await mediator.Send(new GetAllEventsRequest());
        return eventDetails;
    }

    public async Task<EventEntity?> CreateAsync(EventEntity eventEntity)
    {
        var eventDetails = await mediator.Send(new CreateEventRequest() { Event = eventEntity });
        return eventDetails;
    }

    public async Task<EventEntity?> UpdateAsync(EventEntity eventEntity)
    {
        var eventDetails = await mediator.Send(new UpdateEventRequest() { Event = eventEntity });
        return eventDetails;
    }

    public async Task DeleteAsync(Guid id)
    {
        await mediator.Send(new DeleteEventRequest() { Id = id });
    }
}
