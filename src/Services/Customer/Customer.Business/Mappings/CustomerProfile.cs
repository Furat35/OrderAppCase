using AutoMapper;
using Customer.Business.Models.Dtos.Customer;
using Entities = Customer.Entity.Entities;

namespace Customer.Business
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomerCreateDto, Entities.Customer>();
            CreateMap<CustomerUpdateDto, Entities.Customer>().ForMember(_ => _.Id, opt => opt.Ignore());
            CreateMap<Entities.Customer, CustomerListDto>();
        }
    }
}
