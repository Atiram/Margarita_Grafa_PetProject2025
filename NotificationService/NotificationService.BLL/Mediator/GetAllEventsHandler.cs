using MediatR;
using NotificationService.BLL.Mediator.Requests;
using NotificationService.DAL.Entities;
using NotificationService.DAL.Repositories.Interfaces;

namespace NotificationService.BLL.Mediator;
public class GetAllEventsHandler(IEventRepository eventRepository) : IRequestHandler<GetAllEventsRequest, List<EventEntity>?>
{
    public async Task<List<EventEntity>?> Handle(GetAllEventsRequest request, CancellationToken cancellationToken)
    {
        return await eventRepository.GetEventsAsync();
    }
}