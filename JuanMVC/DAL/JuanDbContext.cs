using JuanMVC.Configurations;
using JuanMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JuanMVC.DAL
{
    public class JuanDbContext : IdentityDbContext
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

        public DbSet<Service> Services { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<BasketItem> BasketItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }









        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductSize>().HasKey(x => new { x.ProductId, x.SizeId });
            base.OnModelCreating(modelBuilder);


            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);

        }


    }
}
