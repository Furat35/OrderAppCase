using Customer.Business.Models.Dtos.Customer;

namespace Customer.Business.Services.Constracts
{
    public interface ICustomerService
    {
        Task<Guid> Create(CustomerCreateDto customerDto);
        Task<bool> Update(CustomerUpdateDto customerDto);
        Task<bool> Delete(Guid customerId);
        List<CustomerListDto> Get();
        Task<CustomerListDto> Get(Guid customerId);
        Task<bool> Validate(Guid customerId);
    }
}
