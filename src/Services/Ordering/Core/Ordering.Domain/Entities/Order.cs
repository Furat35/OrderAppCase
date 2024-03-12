using Shared.Entities.Common;

namespace Ordering.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public Address Address { get; set; }
        public Product Product { get; set; }
    }
}
