using JuanMVC.Areas.Manage.ViewModels;
using JuanMVC.DAL;
using JuanMVC.Enums;
using JuanMVC.Models;
using JuanMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index(int page =1, GenderStatus? gender = null , List<int>? categoryId = null, List<int>? brandId = null , List<int>? sizeId = null)
        {
            var query = _context.Products.Include(x => x.Images.Where(x => x.ImageStatus == true)).Include(x=>x.ProductSizes).AsQueryable();


            if (gender!=null)
            {
                query = query.Where(x => x.Gender == gender);
            }

            if (categoryId.Count>0)
            {
                query = query.Where(x => categoryId.Contains(x.CategoryId));
            }


            if (brandId.Count > 0)
            {
                query = query.Where(x => brandId.Contains(x.BrandId));
            }


            if (sizeId.Count > 0)
            {

                foreach (var item in _context.Sizes)
                {
                    query = query.Where(x => sizeId.Contains(item.Id)) ;

                }
            }


            ShopVM vm = new ShopVM()
            {
                Product = PaginatedList<Product>.Create(query, page, 6),
                AllProducts = _context.Products.Include(x => x.Images.Where(x => x.ImageStatus == true)).ToList(),
                Categories = _context.Categories.Include(x => x.Products).ToList(),
                Brands = _context.Brands.Include(x=>x.Products).ToList(),
                Sizes = _context.Sizes.Include(x=>x.ProductSizes).ToList(),
                SelectedGenre = gender,
                SelectedCategory = categoryId,
                SelectedBrand = brandId,
                SelectedSize = sizeId
            };

            return View(vm);
        }
    }
}
