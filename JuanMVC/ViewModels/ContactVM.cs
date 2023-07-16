using System.ComponentModel.DataAnnotations;

namespace JuanMVC.ViewModels
{
    public class ContactVM
    {

        public string AppUserId { get; set; }


        [Required]
        [MaxLength(25)]
        public string FullName { get; set; }


        [Required]
        [MaxLength(15)]
        public string Phone { get; set; }


        [Required]
        [MaxLength(80)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [MaxLength(50)]
        public string Subject { get; set; }



        [MaxLength(200)]
        public string Text { get; set; }
    }
}
