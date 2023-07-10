using JuanMVC.Attributes;
using JuanMVC.Enums;
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

        public GenderStatus  Gender { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal SalePrice { get; set; }


        [Required]
        [Column(TypeName = "money")]
        public decimal CostPrice { get; set; }


        [Required]
        [Column(TypeName = "money")]
        public decimal DiscountedPrice { get; set; }

        [Required]
        [Column(TypeName ="text")]
        public string Description { get; set; }

        [Required]
        public bool StockStatus { get; set; }

        public bool IsNew { get; set; }

        public List<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();

        public List<Image> Images { get; set; } = new List<Image>();


        [NotMapped]
        public List<int> SizeIds { get; set; } = new List<int>();
        [NotMapped]

        public List<int> ImageIds { get; set; } = new List<int>();

        [NotMapped]
        public int PosterImageId { get; set; }


        [NotMapped]
        [FileMaxLength(2097152)]
        [AllowContentType("image/jpeg", "image/png")]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        [FileMaxLength(2097152)]
        [AllowContentType("image/jpeg", "image/png")]
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();



        public Category Category { get; set; }

        public Brand Brand { get; set; }

        public Color Color { get; set; }



    }
}
