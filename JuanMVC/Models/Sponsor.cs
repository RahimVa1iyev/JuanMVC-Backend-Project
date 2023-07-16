using System.ComponentModel.DataAnnotations;

namespace JuanMVC.Models
{
    public class Sponsor
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Logo { get; set; }

    }

}
