using AutoMapper;
using Ordering.Application.Models.Dtos.Audit;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditCreateDto, Audit>();
            CreateMap<Audit, AuditListDto>();
        }
    }
}
