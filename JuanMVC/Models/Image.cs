using System.ComponentModel.DataAnnotations;

namespace JuanMVC.Models
{
    public class Image
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ImageName { get; set; }

        [Required]
        public bool ImageStatus { get; set; }

        public Product Product { get; set; }
    }
}
