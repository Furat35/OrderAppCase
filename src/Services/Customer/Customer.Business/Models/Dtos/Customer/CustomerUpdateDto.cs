using Customer.Business.Models.Dtos.Address;

namespace Customer.Business.Models.Dtos.Customer
{
    public class CustomerUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public AddressUpdateDto Address { get; set; }
    }
}
