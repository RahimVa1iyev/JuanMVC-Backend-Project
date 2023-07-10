using JuanMVC.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanMVC.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string FirstTitle { get; set; }

        [Required]
        [MaxLength(20)]
        public string SecondTitle { get; set; }

        [Required]
        [MaxLength(20)]
        public string BackgroundColor { get; set; }

      
        [MaxLength(100)]
        public string IconImage { get; set; }

        [NotMapped]
        [FileMaxLength(2097152)]
        [AllowContentType("image/jpeg", "image/png")]
        public IFormFile ImageFile { get; set; }
    }
}
