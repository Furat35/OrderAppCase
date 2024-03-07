using Customer.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Entities.Common;
using System.Reflection;
using Entities = Customer.Entity.Entities;
namespace Customer.DataAccess.Repositories.Context
{
    public class EfCustomerContext : DbContext
    {
        public EfCustomerContext(DbContextOptions<EfCustomerContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries()
                            .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entityEntry in entities)
            {
                var baseEntity = (BaseEntity)entityEntry.Entity;

                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        baseEntity.CreatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        baseEntity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Entities.Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
