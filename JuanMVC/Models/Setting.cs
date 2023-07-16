using System.ComponentModel.DataAnnotations;

namespace JuanMVC.Models
{
    public class Setting
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string Key { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; }

    }
}
