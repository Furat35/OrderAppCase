using AutoMapper;
using Ordering.Application.Models.Dtos.Audit;
using Ordering.Application.Services;
using Ordering.Application.Validations.UnitOfWorks;
using Ordering.Domain.Entities;
using Shared.DataAccess.Abstract;

namespace Ordering.Persistence.Services
{
    public class OrderLoggerService : IOrderLoggerService
    {
        private readonly IWriteRepository<Audit> _auditWriteRepository;
        private readonly IReadRepository<Audit> _auditReadRepository;
        private readonly IMapper _mapper;

        public OrderLoggerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _auditReadRepository = unitOfWork.GetReadRepository<Audit>();
            _auditWriteRepository = unitOfWork.GetWriteRepository<Audit>();
            _mapper = mapper;
        }

        public async Task<bool> CreateLog(AuditCreateDto log)
        {
            var audit = _mapper.Map<Audit>(log);
            var effectedRows = await _auditWriteRepository.CreateAsync(audit);

            return effectedRows != 0;
        }

        public List<AuditListDto> GetLogs(DateTime logDate)
        {
            var logs = _auditReadRepository.GetWhere(_ => _.CreatedAt.Year == logDate.Year && _.CreatedAt.Month == logDate.Month && _.CreatedAt.Day == logDate.Day);
            return _mapper.Map<List<AuditListDto>>(logs);
        }
    }
}
