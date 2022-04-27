using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store4Dev.Domain.Entities;

namespace Store4Dev.Data.Mapping
{
    internal sealed class BrandMapping : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("brands");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id).HasColumnName("id");
            builder.Property(b => b.Name).HasColumnName("name").IsRequired();

            builder.Property(b => b.CreatedAt).HasColumnName("created_at");
            builder.Property(b => b.UpdatedAt).HasColumnName("updated_at");
        }
    }
}
