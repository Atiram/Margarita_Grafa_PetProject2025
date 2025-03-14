using MediatR;
using NotificationService.BLL.Mediator.Requests;
using NotificationService.DAL.Entities;
using NotificationService.DAL.Repositories.Interfaces;

namespace NotificationService.BLL.Mediator;
public class GetEventHandler(IEventRepository eventRepository) : IRequestHandler<GetEventRequest, EventEntity?>
{
    public async Task<EventEntity?> Handle(GetEventRequest request, CancellationToken cancellationToken)
    {
        return await eventRepository.GetByIdAsync(request.Id);
    }
}
