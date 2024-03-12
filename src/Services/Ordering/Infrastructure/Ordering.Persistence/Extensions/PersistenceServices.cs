using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.Application.Helpers;
using Ordering.Application.Services;
using Ordering.Application.Validations.UnitOfWorks;
using Ordering.Persistence.ExternalApiServices;
using Ordering.Persistence.ExternalApiServices.Contracts;
using Ordering.Persistence.Helpers;
using Ordering.Persistence.Repositories;
using Ordering.Persistence.Services;
using Ordering.Persistence.UnitOfWorks;
using Shared.DataAccess.Abstract;

namespace Ordering.Persistence.Extensions
{
    public static class PersistenceServices
    {
        public static async Task AddPersistenceServices(this IServiceCollection services, IHostApplicationBuilder builder)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IFileService, FileService>();
            services.AddTransient<IOrderLoggerService, OrderLoggerService>();
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
                });
            });
            services.AddHttpClient<ICustomerService, CustomerService>(c =>
                c.BaseAddress = new Uri(builder.Configuration["OrderAppGatewayUrl"]));
        }
    }
}
