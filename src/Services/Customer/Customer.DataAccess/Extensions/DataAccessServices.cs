using Customer.DataAccess.Repositories;
using Customer.DataAccess.Repositories.Context;
using Customer.DataAccess.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.DataAccess.Abstract;

namespace Customer.DataAccess.Extensions
{
    public static class DataAccessServices
    {
        public static async Task AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Entityframework implementation
            services.AddDbContext<EfCustomerContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("OrderAppMssql")));

            // Scoped servisler
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
        }
    }
}
