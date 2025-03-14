using MediatR;
using NotificationService.BLL.Mediator.Requests;
using NotificationService.DAL.Entities;
using NotificationService.DAL.Repositories.Interfaces;

namespace NotificationService.BLL.Mediator;
public class UpdateEventHandler(IEventRepository eventRepository) : IRequestHandler<UpdateEventRequest, EventEntity?>
{
    public async Task<EventEntity?> Handle(UpdateEventRequest request, CancellationToken cancellationToken)
    {
        return await eventRepository.UpdateAsync(request.Event);
    }
}
