using MediatR;
using NotificationService.DAL.Entities;

namespace NotificationService.BLL.Mediator.Requests;
public class GetEventRequest : IRequest<EventEntity?>
{
    public Guid Id { get; set; }
}
