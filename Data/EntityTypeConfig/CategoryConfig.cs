using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityTypeConfig
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p => p.CategoryName)
                .IsRequired(true)
                .HasMaxLength(100);

            builder.Property(p => p.CreatedDate)
                .IsRequired(true)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }
    }

    public class ProductInformationConfig : IEntityTypeConfiguration<ProductInformation>
    {
        public void Configure(EntityTypeBuilder<ProductInformation> builder)
        {
            builder.Property(p => p.Header)
                .IsRequired(true)
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .IsRequired(true)
                .HasMaxLength(150);

            builder.Property(p => p.CreatedDate)
                .IsRequired(true)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
