using MediatR;
using NotificationService.BLL.Mediator.Requests;
using NotificationService.DAL.Entities;
using NotificationService.DAL.Repositories.Interfaces;

namespace NotificationService.BLL.Mediator;
public class CreateEventHandler(IEventRepository eventRepository) : IRequestHandler<CreateEventRequest, EventEntity>
{
    public async Task<EventEntity> Handle(CreateEventRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Event == null)
            {
                throw new ArgumentNullException(nameof(request.Event), "Event cannot be null.");
            }
            return await eventRepository.CreateAsync(request.Event);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }
}
