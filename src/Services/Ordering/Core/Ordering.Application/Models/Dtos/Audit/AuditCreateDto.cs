using Shared.Enums;

namespace Ordering.Application.Models.Dtos.Audit
{
    public class AuditCreateDto
    {
        public Guid OrderId { get; set; }
        public string Message { get; set; }
        public OperationType Operation { get; set; }
    }
}
