using JuanMVC.Models;

namespace JuanMVC.ViewModels
{
    public class ProductDetailVM
    {
        public Product  Product { get; set; }

        public List<Product> RelatedProducts { get; set; } = new List<Product>();

        public ProductReview Review { get; set; }

    }
}
