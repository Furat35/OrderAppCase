using Ordering.Application.Models.Dtos.Addresses;
using Ordering.Application.Models.Dtos.Products;

namespace Ordering.Application.Models.Dtos.Orders
{
    public class OrderListDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public AddressListDto Address { get; set; }
        public ProductListDto Product { get; set; }
    }
}
