using JuanMVC.Areas.Manage.ViewModels;
using JuanMVC.Enums;
using JuanMVC.Models;

namespace JuanMVC.ViewModels
{
    public class ShopVM
    {
        public PaginatedList<Product> Product { get; set; }

        public List<Product> AllProducts { get; set; }


        public List<Category> Categories { get; set; }

        public List<Brand> Brands { get; set; }

        public List<Size> Sizes { get; set; }

        public GenderStatus? SelectedGenre { get; set; }

        public List<int>? SelectedCategory { get; set; }

        public List<int>? SelectedBrand { get; set; }

        public List<int>? SelectedSize { get; set; }

    }
}
