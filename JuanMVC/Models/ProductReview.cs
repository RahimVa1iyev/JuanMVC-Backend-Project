using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanMVC.Models
{
    public class ProductReview
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string AppUserId { get; set; }

        [Range(1,5)]
        public byte Rate { get; set; }

        [Column(TypeName ="text")]
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public AppUser AppUser { get; set; }

        public  Product Product { get; set; }


    }
}
