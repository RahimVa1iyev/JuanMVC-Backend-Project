using System.ComponentModel.DataAnnotations;

namespace JuanMVC.Models
{
    public class Size
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public List<ProductSize> ProductSizes { get; set; }
    }
}
