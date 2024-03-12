using Ordering.Application.Models.Dtos;

namespace Ordering.Persistence.ExternalApiServices.Contracts
{
    public interface ICustomerService
    {
        Task<CustomerListDto> GetCustomerById(Guid id);
    }
}
