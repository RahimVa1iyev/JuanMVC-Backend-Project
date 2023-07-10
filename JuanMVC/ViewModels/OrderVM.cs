using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanMVC.ViewModels
{
    public class OrderVM
    {
        [Required]
        [MaxLength(25)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(65)]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(65)]
        public string Address { get; set; }

        [Column(TypeName = "text")]
        public string Text { get; set; }
    }
}
