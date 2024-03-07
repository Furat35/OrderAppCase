namespace Customer.Business.Models.Dtos.Address
{
    public class AddressListDto
    {
        public Guid Id { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int CityCode { get; set; }
    }
}
