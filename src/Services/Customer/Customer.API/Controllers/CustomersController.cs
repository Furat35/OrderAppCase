using Customer.Business.Models.Dtos.Customer;
using Customer.Business.Services.Constracts;
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

        /// <summary>
        /// Get customers
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(CustomerListDto), StatusCodes.Status200OK)]
        public IActionResult GetCustomers()
        {
            var customers = _customerService.Get();
            return Ok(customers);
        }

        /// <summary>
        /// Get customer with give id 
        /// </summary>
        /// <param name="customerId">Customer id</param>
        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(CustomerListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomers(Guid customerId)
        {
            var customer = await _customerService.Get(customerId);
            return Ok(customer);
        }

        /// <summary>
        /// Create customer 
        /// </summary>
        /// <param name="customer">Customer properties needed to create the customer</param>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDto customer)
        {
            var customerId = await _customerService.Create(customer);
            return Ok(new { id = customerId });
        }

        /// <summary>
        /// Update customer 
        /// </summary>
        /// <param name="customer">Customer properties needed to update the customer</param>
        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerUpdateDto customer)
        {
            var isUpdated = await _customerService.Update(customer);
            return Ok(isUpdated);
        }

        /// <summary>
        /// Delete customer 
        /// </summary>
        /// <param name="customerId">Customer id</param>
        [HttpDelete("{customerId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            var isDeleted = await _customerService.Delete(customerId);
            return Ok(isDeleted);
        }
    }
}
