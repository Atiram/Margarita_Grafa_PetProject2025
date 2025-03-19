﻿using MediatR;
using NotificationService.BLL.Mediator.Requests;
using NotificationService.BLL.Models;
using NotificationService.BLL.Services.Interfaces;
using NotificationService.DAL.Entities;
using NotificationService.DAL.Enums;

namespace NotificationService.BLL.Services;
public class EventService(IMediator mediator, IEmailService emailService) : IEventService
{
    private const string emailSubject = "Your appointment is created";
    private const string emailMessageTemplate = "Your appointment with ID {0} is created!";

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
            var createEventMail = new CreateEventMail
            {
                Email = eventDetails.Metadata,
                Subject = emailSubject,
                Message = string.Format(emailMessageTemplate, eventDetails.Id),
                OrderDate = DateTime.UtcNow
            };
            await emailService.SendEmailAsync(createEventMail);
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
