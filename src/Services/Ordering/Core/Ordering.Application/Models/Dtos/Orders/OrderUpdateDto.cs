using Ordering.Application.Models.Dtos.Addresses;

namespace Ordering.Application.Models.Dtos.Orders
{
    public class OrderUpdateDto
    {
        public Guid Id { get; set; }
        public AddressUpdateDto Address { get; set; }
    }
}
