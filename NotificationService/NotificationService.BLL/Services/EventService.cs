using Clinic.Domain;
using MediatR;
using NotificationService.BLL.Mediator.Requests;
using NotificationService.BLL.Services.Interfaces;
using NotificationService.DAL.Entities;
using NotificationService.DAL.Enums;

namespace NotificationService.BLL.Services;
public class EventService(IMediator mediator, IEmailService emailService) : IEventService
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

    public async Task<EventEntity?> CreateAsync(CreateEventMail request)
    {
        var eventEntity = new EventEntity
        {
            Type = EventType.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        eventEntity.SetMetadata(nameof(CreateEventMail.Email), request.Email);

        var eventDetails = await mediator.Send(new CreateEventRequest { Event = eventEntity });

        if (eventDetails?.Metadata != null)
        {
            await emailService.SendEmailAsync(request);
        }
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
