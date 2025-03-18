using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using MediatR;
using NotificationService.BLL.Mediator.Requests;
using NotificationService.BLL.Models;
using NotificationService.BLL.Services.Interfaces;
using NotificationService.DAL.Entities;

namespace NotificationService.BLL.Services;
public class EventService(IMediator mediator, IEmailService emailService) : IEventService
{
    private const string emailSubject = "Your appointment is created";
    private const string emailMessageTemplate = "Your appointment with ID {0} is created!";
    private string? _metadata;

    public string? Metadata
    {
        get => _metadata;
        set => _metadata = value;
    }

    [NotMapped]
    public Dictionary<string, object> MetadataDict
    {
        get => string.IsNullOrEmpty(_metadata)
            ? new Dictionary<string, object>()
            : JsonSerializer.Deserialize<Dictionary<string, object>>(_metadata)
              ?? new Dictionary<string, object>();
        set => _metadata = JsonSerializer.Serialize(value);
    }

    public T? GetMetadata<T>(string key)
    {
        if (MetadataDict.TryGetValue(key, out var value))
        {
            var json = JsonSerializer.Serialize(value);
            if (!string.IsNullOrEmpty(json))
            {
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        return default;
    }

    public void SetMetadata<T>(string key, T value)
    {
        var dict = MetadataDict;
        dict[key] = value;
        MetadataDict = dict;
    }
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
        var eventDetails = await mediator.Send(new CreateEventRequest { Event = eventEntity });

        if (eventDetails?.Metadata != null)
        {
            var createEventMail = new CreateEventMail
            {
                Email = eventDetails.Metadata, //.GetMetadata<string>("email"),
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
