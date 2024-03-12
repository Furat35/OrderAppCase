using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Persistence.EntityTypeConfigurations
{
    internal class AddressTypeConfigurations : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(p => p.Id);

            builder.Property(a => a.AddressLine)
                .IsRequired();

            builder.Property(a => a.City)
                .IsRequired();

            builder.Property(a => a.Country)
                .IsRequired();

            builder.Property(a => a.CityCode)
                .IsRequired();

            builder.Ignore(a => a.CreatedAt);
            builder.Ignore(a => a.UpdatedAt);
        }
    }
}
