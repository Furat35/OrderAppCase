using Shared.Entities.Common;
using Shared.Enums;

namespace Ordering.Domain.Entities
{
    public class Audit : BaseEntity
    {
        public string Message { get; set; }
        public Guid OrderId { get; set; }
        public OperationType Operation { get; set; }
    }
}
