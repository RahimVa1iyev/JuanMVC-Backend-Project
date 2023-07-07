using JuanMVC.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanMVC.Models
{
    public class Slider
    {

        public int Id { get; set; }
       
        public int Order { get; set; }

        [Required]
        [MaxLength(25)]
        public string FirstTitle { get; set; }

        [Required]
        [MaxLength(25)]
        public string SecondTitle { get; set; }

        [Required]
        [MaxLength(65)]
        public string Description { get; set; }

        [Required]
        [MaxLength(25)]
        public string ButtonText { get; set; }

        [Required]
        [MaxLength(25)]
        public string ButtonUrl { get; set; }

        
        [MaxLength(100)]
        public string Image { get; set; }

        [NotMapped]
        [FileMaxLength(2097152)]
        [AllowContentType("image/jpeg","image/png")]
        public IFormFile ImageFile { get; set; }

    }
}
