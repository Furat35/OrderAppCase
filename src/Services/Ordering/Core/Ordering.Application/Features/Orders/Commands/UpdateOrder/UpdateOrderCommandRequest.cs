using MediatR;
using Ordering.Application.Models.Dtos.Addresses;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandRequest : IRequest<UpdateOrderCommandResponse>
    {
        public Guid Id { get; set; }
        public AddressUpdateDto Address { get; set; }
    }
}
