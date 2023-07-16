using Microsoft.AspNetCore.Identity;

namespace JuanMVC.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

        public bool IsAdmin { get; set; }

        public List<BasketItem>  BasketItems { get; set; }

        public string ConnectionId { get; set; }

        public bool IsOnline { get; set; }

        public DateTime LastOnlineAt { get; set; }

        public List<UserContact> Messages { get; set; }

    }
}
