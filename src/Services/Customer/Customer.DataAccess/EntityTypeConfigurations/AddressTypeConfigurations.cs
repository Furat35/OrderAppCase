using Customer.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.DataAccess.EntityTypeConfigurations
{
    public class AddressTypeConfigurations : IEntityTypeConfiguration<Address>
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
