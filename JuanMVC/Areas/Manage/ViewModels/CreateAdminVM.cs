using System.ComponentModel.DataAnnotations;

namespace JuanMVC.Areas.Manage.ViewModels
{
    public class CreateAdminVM
    {
        [Required]
        [MaxLength(25)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(25)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(25)]
        public string Password { get; set; }

        [Required]
        [MaxLength(25)]
        public string Role { get; set; }

    }
}
