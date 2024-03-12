using MediatR;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandRequest : IRequest<DeleteOrderCommandResponse>
    {
        public Guid OrderId { get; set; }
    }
}
