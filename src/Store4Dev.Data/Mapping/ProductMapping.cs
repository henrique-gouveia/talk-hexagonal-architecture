using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store4Dev.Domain.Entities;

namespace Store4Dev.Data.Mapping
{
    internal sealed class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id");

            builder.Property(p => p.BrandId).HasColumnName("brand_id");
            builder
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Name).HasColumnName("name").IsRequired();
            builder.Property(p => p.CostPrice).HasColumnName("cost_price");
            builder.Property(p => p.SalePrice).HasColumnName("sale_price");

            builder.Property(p => p.CurrentStock).HasColumnName("current_stock");
            builder.Property(p => p.MinStock).HasColumnName("min_stock");
            builder.Property(p => p.Active).HasColumnName("active");

            builder.Property(p => p.CreatedAt).HasColumnName("created_at");
            builder.Property(p => p.UpdatedAt).HasColumnName("updated_at");
        }
    }
}
