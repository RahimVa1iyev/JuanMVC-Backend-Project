using System.ComponentModel.DataAnnotations;

namespace JuanMVC.ViewModels
{
    public class ForgotPasswordVM
    {


        [Required]
        [MaxLength(25)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
