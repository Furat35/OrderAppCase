using Customer.Business.Models.Dtos.Address;

namespace Customer.Business.Models.Dtos.Customer
{
    public class CustomerCreateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public AddressCreateDto Address { get; set; }
    }
}
