using Shared.Entities.Common;

namespace Ordering.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string? ImageUrl { get; set; }
        public string Name { get; set; }
        public Order Order { get; set; }
    }
}
