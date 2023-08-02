using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityTypeConfig
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.ProductName)
                .IsRequired(true)
                .HasMaxLength(150);

            builder.Property(p => p.Price)
                .IsRequired(true)
                .HasColumnType("decimal")
                .HasPrecision(6,2);

            builder.Property(p => p.OldPrice)
                .IsRequired(false)
                .HasColumnType("decimal")
                .HasPrecision(6, 2);

            builder.Property(p => p.CreatedDate)
                .IsRequired(true)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
