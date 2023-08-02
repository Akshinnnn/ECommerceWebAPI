using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityTypeConfig
{
    public class ProductionCompanyConfig : IEntityTypeConfiguration<ProductionCompany>
    {
        public void Configure(EntityTypeBuilder<ProductionCompany> builder)
        {
            builder.Property(p => p.CompanyName)
                .IsRequired(true)
                .HasMaxLength(100);

            builder.Property(p => p.CreatedDate)
                .IsRequired(true)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
