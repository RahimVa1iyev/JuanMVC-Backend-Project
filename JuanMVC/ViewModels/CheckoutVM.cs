namespace JuanMVC.ViewModels
{
    public class CheckoutVM
    {
        public List<CheckoutItemVM> Items { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderVM Order { get; set; }

    }
}
