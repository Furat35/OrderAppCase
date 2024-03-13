using EventBus.Message.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Services;
using Ordering.Application.UnitOfWorks;
using Ordering.Logger.Consumers;
using Ordering.Persistence.ExternalApiServices;
using Ordering.Persistence.ExternalApiServices.Contracts;
using Ordering.Persistence.Repositories;
using Ordering.Persistence.Repositories.Context;
using Ordering.Persistence.Services;
using Ordering.Persistence.UnitOfWorks;
using Shared.DataAccess.Abstract;

namespace Ordering.Logger.Extensions
{
    public static class LoggerServices
    {
        public static void AddLoggerServices(this IServiceCollection services, IHostApplicationBuilder builder)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddTransient<IOrderLoggerService, OrderLoggerService>();
            services.AddDbContext<EfOrderingContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("OrderingDbMssql")));
            services.AddHttpClient<ICustomerService, CustomerService>(c =>
              c.BaseAddress = new Uri(builder.Configuration["OrderAppGatewayUrl"]));

            services.AddMassTransit(config =>
            {
                config.AddConsumer<OrderLogConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
                    cfg.ReceiveEndpoint(EventBusConstants.OrderLogEvent, c =>
                    {
                        c.ConfigureConsumer<OrderLogConsumer>(ctx);
                    });
                });
            });
        }
    }
}
