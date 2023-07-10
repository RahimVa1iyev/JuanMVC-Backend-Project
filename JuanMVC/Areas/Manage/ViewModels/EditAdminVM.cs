using System.ComponentModel.DataAnnotations;

namespace JuanMVC.Areas.Manage.ViewModels
{
    public class EditAdminVM
    {

        [Required]
        [MaxLength(25)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(25)]
        public string UserName { get; set; }

     
        [Required]
        [MaxLength(25)]
        public string Role { get; set; }
    }
}
