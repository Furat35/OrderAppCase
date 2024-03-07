using AutoMapper;
using Customer.Business.Models.Dtos.Address;
using Customer.Entity.Entities;

namespace Customer.Business.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressCreateDto, Address>();
            CreateMap<AddressUpdateDto, Address>().ForMember(_ => _.Id, opt => opt.Ignore());
            CreateMap<Address, AddressListDto>();
        }
    }
}
