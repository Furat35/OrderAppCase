using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Persistence.EntityTypeConfigurations
{
    public class ProductTypeConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(c => c.Id);

            builder.Property(_ => _.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Ignore(a => a.CreatedAt);
            builder.Ignore(a => a.UpdatedAt);
        }
    }
}
