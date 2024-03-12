using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ordering.Application.Extensions
{
    public static class ApplicationServices
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
