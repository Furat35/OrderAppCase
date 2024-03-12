using Shared.Enums;

namespace Ordering.Application.Models.Dtos.Audit
{
    public class AuditListDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public OperationType Operation { get; set; }
    }
}
