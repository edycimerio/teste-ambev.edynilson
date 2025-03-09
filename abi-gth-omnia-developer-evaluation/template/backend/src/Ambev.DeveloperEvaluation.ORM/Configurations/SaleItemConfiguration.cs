using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Configurations;

/// <summary>
/// Entity Framework configuration for the SaleItem entity
/// </summary>
public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ProductCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.Discount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalPrice)
            .IsRequired()
            .HasPrecision(18, 2);
    }
}
