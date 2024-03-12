using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

namespace Ordering.Persistence.Repositories.Context
{
    public class ContextSeed
    {
        private readonly IServiceProvider _serviceProvider;

        public ContextSeed(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SeedDatabaseAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EfOrderingContext>();
                context.Database.EnsureCreated();
                if (!context.Orders.Any() || !context.Products.Any() || !context.Addresses.Any())
                {
                    await context.Orders.AddRangeAsync(SeedCustomersTable());
                    await context.SaveChangesAsync();
                }
            }
        }
        private List<Order> SeedCustomersTable()
            => new List<Order>
            {
                new (){ Id = Guid.Parse("77D8732C-4B73-4FCC-ADB0-F9E8A9F9CB01") ,CustomerId = Guid.Parse("E2D2E7D0-4C92-490C-B4D2-C1A85ADBE8F8"), Quantity = 5, Price = 35,
                    Status = Enum.GetName(OrderStatus.Pending), Address = new (){Id =  Guid.Parse("66D8732C-4B73-4FCC-ADB0-F9E8A9F9CB02"), AddressLine = "home", City = "izmir",
                    Country = "Turkiye", CityCode = 35 }, Product = new(){ Id = Guid.Parse("13D8732C-4B73-4FCC-ADB0-F9E8A9F9CB02"),ImageUrl = "iphone.png", Name = "iphone" }, CreatedAt = DateTime.Now},
                new (){ Id = Guid.Parse("37D8732C-4B73-4FCC-ADB0-F9E8A9F9CB01") ,CustomerId = Guid.Parse("E2D2E7D0-4C92-490C-B4D2-C1A85ADBE8F8"), Quantity = 2, Price = 1200,
                    Status = Enum.GetName(OrderStatus.Pending), Address = new (){Id =  Guid.Parse("55D8732C-4B73-4FCC-ADB0-F9E8A9F9CB44"), AddressLine = "office", City = "istanbul",
                    Country = "Turkiye", CityCode = 34 }, Product = new(){ Id = Guid.Parse("89D8732C-4B73-4FCC-ADB0-F9E8A9F9CB11"),ImageUrl = "tv.png", Name = "samsung" }, CreatedAt = DateTime.Now}
            };
    }
}
