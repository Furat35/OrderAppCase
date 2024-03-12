using Ordering.Application.Models.Dtos.Addresses;

namespace Ordering.Application.Models.Dtos
{
    public class CustomerListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public AddressListDto Address { get; set; }
    }
}
