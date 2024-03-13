using Microsoft.EntityFrameworkCore;
using Ordering.Persistence.Repositories.Context;

namespace Ordering.API.Extensions
{
    public static class ApiServices
    {
        public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHttpContextAccessor();

            // EntityFramework configurations
            services.AddDbContext<EfOrderingContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("OrderingDbMssql")));

            // Cors implementation
            services.AddCors(options =>
            {
                options.AddPolicy("AllowedOrigins",
                    builder =>
                    {
                        builder.WithOrigins("*")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
        }
    }
}
