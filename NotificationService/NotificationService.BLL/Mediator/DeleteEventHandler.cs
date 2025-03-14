using MediatR;
using NotificationService.BLL.Mediator.Requests;
using NotificationService.DAL.Repositories.Interfaces;

namespace NotificationService.BLL.Mediator;
internal class DeleteEventHandler(IEventRepository eventRepository) : IRequestHandler<DeleteEventRequest>
{
    public async Task Handle(DeleteEventRequest request, CancellationToken cancellationToken)
    {
        await eventRepository.DeleteAsync(request.Id);
    }
}
