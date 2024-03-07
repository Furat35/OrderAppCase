using Customer.Entity.Entities;
using Microsoft.Extensions.DependencyInjection;
using Entities = Customer.Entity.Entities;

namespace Customer.DataAccess.Repositories.Context
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
                var context = scope.ServiceProvider.GetRequiredService<EfCustomerContext>();
                context.Database.EnsureCreated();
                if (!context.Customers.Any())
                {
                    await context.Customers.AddRangeAsync(SeedCustomersTable());
                    await context.SaveChangesAsync();
                }
            }
        }
        private List<Entities.Customer> SeedCustomersTable()
            => new List<Entities.Customer>
            {
                new Entities.Customer{ Id = Guid.Parse("E2D2E7D0-4C92-490C-B4D2-C1A85ADBE8F8"), Email = "kaan@gmail.com", Name = "Kaan", CreatedAt = DateTime.Now ,
                        Address = new Address{ Id =  Guid.Parse("A5D2E7D0-4C92-490C-B4D2-C1A85ADBE8F8"), AddressLine= "Home", City = "izmir", Country = "Turkey",
                        CityCode = 35, CreatedAt = DateTime.Now } },

                new Entities.Customer{ Id = Guid.Parse("F89B32D7-3E50-41D3-8533-302A00D84393"), Email = "ahmet@gmail.com", Name = "Ahmet", CreatedAt = DateTime.Now ,
                        Address = new Address{ Id =  Guid.Parse("55D2E7D0-4C92-490C-B4D2-C1A85ADBE8F8"), AddressLine= "Ofice", City = "istanbul", Country = "Turkey",
                        CityCode = 6,  CreatedAt = DateTime.Now } },

                new Entities.Customer{ Id = Guid.Parse("32D2E7D0-4C92-490C-B4D2-C1A85ADBE8F8"), Email = "methmet@gmail.com", Name = "Mehmet", CreatedAt = DateTime.Now ,
                        Address = new Address{ Id =  Guid.Parse("FCA29AA0-3FB2-4659-96C6-7CB79BD45501"), AddressLine= "Home", City = "Ankara", Country = "Turkey", CityCode = 35,
                        CreatedAt = DateTime.Now } },
            };
    }
}
