using Shared.Entities.Common;

namespace Ordering.Domain.Entities
{
    public class Address : BaseEntity
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int CityCode { get; set; }
        public Order Order { get; set; }
    }
}
