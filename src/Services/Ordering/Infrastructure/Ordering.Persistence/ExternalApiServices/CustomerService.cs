using Ordering.Application.Models.Dtos;
using Ordering.Persistence.ExternalApiServices.Contracts;
using Shared.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ordering.Persistence.ExternalApiServices
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomerListDto> GetCustomerById(Guid id)
        {
            var response = await _httpClient.GetAsync($"customer/{id}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<CustomerListDto>();

            string errorContent = await response.Content.ReadAsStringAsync();
            var errorDetails = JsonSerializer.Deserialize<ErrorDetail>(errorContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            throw new HttpRequestException(message: errorDetails.ErrorMessage, null, statusCode: (HttpStatusCode)(errorDetails.StatusCode));
        }
    }
}
