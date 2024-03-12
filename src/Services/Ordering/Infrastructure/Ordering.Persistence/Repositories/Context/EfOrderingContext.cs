using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Shared.Entities.Common;
using System.Reflection;

namespace Ordering.Persistence.Repositories.Context
{
    public class EfOrderingContext : DbContext
    {
        public EfOrderingContext(DbContextOptions<EfOrderingContext> options) : base(options)
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


        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Audit> Audits { get; set; }
    }
}
