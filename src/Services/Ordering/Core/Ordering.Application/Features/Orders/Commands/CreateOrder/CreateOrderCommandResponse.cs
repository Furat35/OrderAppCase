using MediatR;

namespace Ordering.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandResponse : IRequest<CreateOrderCommandResponse>
    {
        public Guid OrderId { get; set; }
    }
}
