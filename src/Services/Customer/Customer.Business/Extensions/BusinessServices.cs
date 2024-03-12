using Customer.Business.Services;
using Customer.Business.Services.Constracts;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Customer.Business.Extensions
{
    public static class BusinessServices
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<ICustomerService, CustomerService>();
        }
    }
}
