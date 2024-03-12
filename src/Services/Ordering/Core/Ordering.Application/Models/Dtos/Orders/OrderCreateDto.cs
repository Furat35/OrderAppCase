using Ordering.Application.Models.Dtos.Products;

namespace Ordering.Application.Models.Dtos.Orders
{
    public class OrderCreateDto
    {
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public ProductAddDto Product { get; set; }
    }
}
