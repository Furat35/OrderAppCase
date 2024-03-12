using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Persistence.EntityTypeConfigurations
{
    public class OrderTypeConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(c => c.Id);

            builder.Property(_ => _.CustomerId)
                .IsRequired();

            builder.Property(_ => _.Quantity)
                .IsRequired();

            builder.Property(_ => _.Price)
                .IsRequired();

            builder.Property(_ => _.Status)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(_ => _.Address)
                .WithOne(_ => _.Order)
                .HasForeignKey<Address>(_ => _.Id)
                .IsRequired();

            builder.HasOne(_ => _.Product)
                .WithOne(_ => _.Order)
                .HasForeignKey<Product>(_ => _.Id)
                .IsRequired();
        }
    }
}
