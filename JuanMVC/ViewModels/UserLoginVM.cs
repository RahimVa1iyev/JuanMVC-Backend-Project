using System.ComponentModel.DataAnnotations;

namespace JuanMVC.ViewModels
{
    public class UserLoginVM
    {

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
