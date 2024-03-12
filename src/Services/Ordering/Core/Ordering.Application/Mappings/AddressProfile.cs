using AutoMapper;
using Ordering.Application.Models.Dtos.Addresses;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressListDto>().ReverseMap();
            CreateMap<AddressUpdateDto, Address>().ReverseMap();
        }
    }
}
