using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanMVC.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        public int ColorId { get; set; }

        [Required]
        [MaxLength(50)]
        public string  Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountedPrice { get; set; }

        [Required]
        [Column(TypeName ="text")]
        public string Description { get; set; }

        [Required]
        public bool StockStatus { get; set; }

        public List<ProductSize> ProductSizes { get; set; }

        public List<Image> Images { get; set; }

        public Category Category { get; set; }

        public Brand Brand { get; set; }

        public Color Color { get; set; }



    }
}
