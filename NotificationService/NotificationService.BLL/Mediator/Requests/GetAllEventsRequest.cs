using MediatR;
using NotificationService.DAL.Entities;

namespace NotificationService.BLL.Mediator.Requests;
public class GetAllEventsRequest : IRequest<List<EventEntity>?>;
