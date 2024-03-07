using Customer.Business.Models.Dtos.Customer;
using Customer.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = _customerService.Get();
            return Ok(customers);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomer(Guid customerId)
        {
            var customer = await _customerService.Get(customerId);
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDto customer)
        {
            var customerId = await _customerService.Create(customer);
            return Ok(new { id = customerId });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerUpdateDto customer)
        {
            var isUpdated = await _customerService.Update(customer);
            return Ok(isUpdated);
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            var isDeleted = await _customerService.Delete(customerId);
            return Ok(isDeleted);
        }
    }
}
