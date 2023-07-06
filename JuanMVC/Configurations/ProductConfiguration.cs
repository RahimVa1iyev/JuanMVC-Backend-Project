using JuanMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuanMVC.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Description).IsRequired().HasColumnType("text");
            builder.Property(x => x.SalePrice).IsRequired().HasColumnType("money");
            builder.Property(x => x.CostPrice).IsRequired().HasColumnType("money");
            builder.Property(x => x.DiscountedPrice).IsRequired().HasColumnType("money");
            builder.Property(x => x.StockStatus).IsRequired();
            builder.HasOne(x => x.Category).WithMany(x => x.Products).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Brand).WithMany(x => x.Products).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Color).WithMany(x => x.Products).OnDelete(DeleteBehavior.NoAction);




        }
    }
}
