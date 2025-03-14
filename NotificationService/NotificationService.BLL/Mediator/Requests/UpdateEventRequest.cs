using MediatR;
using NotificationService.DAL.Entities;

namespace NotificationService.BLL.Mediator.Requests;
public class UpdateEventRequest : IRequest<EventEntity?>
{
    public EventEntity Event { get; set; }
}
