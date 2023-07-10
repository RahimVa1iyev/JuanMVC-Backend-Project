using System.ComponentModel.DataAnnotations;

namespace JuanMVC.Areas.Manage.ViewModels
{
    public class AdminLoginVM
    {
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(20)]
        [DataType(DataType.Password)]

        public string Password { get; set; }
    }
}
