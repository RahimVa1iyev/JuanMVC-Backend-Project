using System.ComponentModel.DataAnnotations;

namespace JuanMVC.ViewModels
{
    public class MemberAccountProfileVM
    {
        [Required]
        [MaxLength(30)]

        public string FullName { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

       
        [MaxLength(25)]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }

        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
    }
}
