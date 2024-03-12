using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Persistence.EntityTypeConfigurations
{
    public class AuditTypeConfigurations : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.ToTable("Audits");

            builder.HasKey(p => p.Id);

            builder.Property(a => a.Message)
                .IsRequired();

            builder.Ignore(_ => _.UpdatedAt);

        }
    }
}