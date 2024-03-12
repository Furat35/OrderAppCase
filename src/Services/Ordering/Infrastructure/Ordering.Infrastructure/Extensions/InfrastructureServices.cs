using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.Application.Mail;
using Ordering.Infrastructure.BackgroundServices;
using Ordering.Infrastructure.Mail;

namespace Ordering.Infrastructure.Extensions
{
    public static class InfrastructureServices
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddHostedService<OrderLogEmailSender>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IHostedService, OrderLogEmailSender>();
        }
    }
}
