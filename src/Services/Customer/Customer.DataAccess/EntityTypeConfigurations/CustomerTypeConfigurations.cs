using Customer.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Customer.Entity.Entities;

namespace Customer.DataAccess.EntityTypeConfigurations
{
    public class CustomerTypeConfigurations : IEntityTypeConfiguration<Entities.Customer>
    {
        public void Configure(EntityTypeBuilder<Entities.Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);

            builder.HasOne(_ => _.Address)
                .WithOne(_ => _.Customer)
                .HasForeignKey<Address>(_ => _.Id)
                .IsRequired();

            builder.Property(c => c.Name)
                .IsRequired();

            builder.Property(c => c.Email)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .IsRequired();
        }
    }
}
