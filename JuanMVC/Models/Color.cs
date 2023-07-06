using System.ComponentModel.DataAnnotations;

namespace JuanMVC.Models
{
    public class Color
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]

        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
