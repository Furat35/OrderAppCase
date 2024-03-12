using MediatR;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandRequest : IRequest<UpdateOrderStatusCommandResponse>
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; }
    }
}
