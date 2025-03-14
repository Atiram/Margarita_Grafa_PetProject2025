using MediatR;

namespace NotificationService.BLL.Mediator.Requests;
public class DeleteEventRequest : IRequest
{
    public Guid Id { get; set; }
}
