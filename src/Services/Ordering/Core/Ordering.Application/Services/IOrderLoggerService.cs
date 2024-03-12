using Ordering.Application.Models.Dtos.Audit;

namespace Ordering.Application.Services
{
    public interface IOrderLoggerService
    {
        Task<bool> CreateLog(AuditCreateDto log);
        List<AuditListDto> GetLogs(DateTime logDate);
    }
}
