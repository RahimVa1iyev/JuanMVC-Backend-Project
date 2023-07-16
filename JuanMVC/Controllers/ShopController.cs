using JuanMVC.Areas.Manage.ViewModels;
using JuanMVC.DAL;
using JuanMVC.Enums;
using JuanMVC.Models;
using JuanMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JuanMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly JuanDbContext _context;

        public ShopController(JuanDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page =1, GenderStatus? gender = null , List<int>? categoryId = null, List<int>? brandId = null , List<int>? sizeId = null,decimal? minPrice =null, decimal? maxPrice = null,string sort = "A_to_Z")
        {
            var query = _context.Products.Include(x => x.Images.Where(x => x.ImageStatus == true)).Include(x=>x.ProductSizes).AsQueryable();

            ShopVM vm = new ShopVM();

            vm.MinPrice = (int)query.Min(x => x.SalePrice);
            vm.MaxPrice = (int)query.Max(x => x.SalePrice);



            if (minPrice != null && maxPrice != null)
                query = query.Where(x => (int)x.SalePrice >= (int)minPrice && (int)x.SalePrice <= (int)maxPrice);


            if (gender!=null)
                query = query.Where(x => x.Gender == gender);

            if (categoryId.Count>0)
                query = query.Where(x => categoryId.Contains(x.CategoryId));

            if (brandId.Count > 0)
                query = query.Where(x => brandId.Contains(x.BrandId));

            if (sizeId.Count > 0)
                query = query.Where(x => x.ProductSizes.Any(ps => sizeId.Contains(ps.SizeId)));

         
            switch (sort)
            {
                case "A_to_Z":
                    query = query.OrderBy(x => x.Name);
                    break;
                case "Z_to_A":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "High_to_Low":
                    query = query.OrderByDescending(x => x.SalePrice);
                    break;
                case "Low_to_High":
                    query = query.OrderBy(x => x.SalePrice);
                    break;
                default:
                    break;
            }

            vm.SelectLists = new List<SelectListItem>
            {
                new SelectListItem("Name : (A-Z)", "A_to_Z",sort=="A_to_Z"),
                new SelectListItem("Name : (Z-A)", "Z_to_A",sort=="Z_to_A"),
                new SelectListItem("Price : (High-Low)", "High_to_Low",sort=="High_to_Low"),
                new SelectListItem("Price : (Low-High)", "Low_to_High",sort=="Low_to_High"),

            };


                vm.Product = PaginatedList<Product>.Create(query, page, 6);
                vm.AllProducts = _context.Products.Include(x => x.Images.Where(x => x.ImageStatus == true)).ToList();
                vm.Categories = _context.Categories.Include(x => x.Products).ToList();
                vm.Brands = _context.Brands.Include(x => x.Products).ToList();
                vm.Sizes = _context.Sizes.Include(x => x.ProductSizes).ToList();
                vm.SelectedGenre = gender;
                vm.SelectedCategory = categoryId;
                vm.SelectedBrand = brandId;
                vm.SelectedSize = sizeId;
                vm.SelectedMinPrice = (int)(minPrice == null ? vm.MinPrice : (decimal)minPrice);
                vm.SelectedMaxPrice = (int)(maxPrice == null ? vm.MaxPrice : (decimal)maxPrice);



            return View(vm);
        }
    }
}
