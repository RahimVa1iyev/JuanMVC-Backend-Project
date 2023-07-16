using JuanMVC.Models;

namespace JuanMVC.ViewModels
{
    public class ProfileVM
    {
        public MemberAccountProfileVM MemberProfile { get; set; }

        public List<Order>  Orders  { get; set; } = new List<Order>();
    }
}
