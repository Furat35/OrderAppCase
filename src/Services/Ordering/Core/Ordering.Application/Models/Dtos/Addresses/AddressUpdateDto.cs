namespace Ordering.Application.Models.Dtos.Addresses
{
    public class AddressUpdateDto
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int CityCode { get; set; }
    }
}
