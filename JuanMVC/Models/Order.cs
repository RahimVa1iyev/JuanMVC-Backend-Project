using JuanMVC.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanMVC.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }


        public DateTime CreatedAt { get; set; }

        [Required]
        [MaxLength(25)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(65)]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        public  string Phone { get; set; }

        [Required]
        [MaxLength(65)]
        public string Address { get; set; }

        [Column(TypeName ="text")]
        public string Text { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public AppUser User { get; set; }

        public List<OrderItem> OrderItems { get; set; }




    }
}
