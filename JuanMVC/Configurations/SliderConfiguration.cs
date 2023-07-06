using JuanMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuanMVC.Configurations
{
    public class SliderConfiguration : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            builder.Property(x=>x.FirstTitle).IsRequired().HasMaxLength(30);
            builder.Property(x=>x.SecondTitle).IsRequired().HasMaxLength(40);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(100);
            builder.Property(x=>x.ButtonText).IsRequired().HasMaxLength(20);
            builder.Property(x => x.ButtonUrl).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Image).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Order).IsRequired().HasAnnotation("Range" , new[] {1,100});

        }
    }
}
