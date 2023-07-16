using JuanMVC.Areas.Manage.ViewModels;
using JuanMVC.Enums;
using JuanMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JuanMVC.ViewModels
{
    public class ShopVM
    {
        public PaginatedList<Product> Product { get; set; }

        public List<Product> AllProducts { get; set; }


        public List<Category> Categories { get; set; }

        public List<Brand> Brands { get; set; }

        public List<Size> Sizes { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public decimal SelectedMinPrice { get; set; }

        public decimal SelectedMaxPrice { get; set; }

         public List<SelectListItem> SelectLists { get; set; }


        public GenderStatus? SelectedGenre { get; set; }

        public List<int>? SelectedCategory { get; set; }

        public List<int>? SelectedBrand { get; set; }

        public List<int>? SelectedSize { get; set; }



    }
}
