using Shared.Entities.Common;

namespace Customer.Entity.Entities
{
    public class Address : BaseEntity
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int CityCode { get; set; }
        public Customer Customer { get; set; }
    }
}
