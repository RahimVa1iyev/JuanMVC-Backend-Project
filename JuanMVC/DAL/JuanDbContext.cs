using JuanMVC.Configurations;
using JuanMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace JuanMVC.DAL
{
    public class JuanDbContext : DbContext
    {

        public JuanDbContext(DbContextOptions<JuanDbContext> options) : base(options) { }




        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Size> Sizes { get; set; }

        public DbSet<ProductSize> ProductSizes { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Slider> Sliders { get; set; }









        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductSize>().HasKey(x => new { x.ProductId, x.SizeId });
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);

        }


    }
}
