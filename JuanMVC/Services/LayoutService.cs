using JuanMVC.DAL;
using JuanMVC.Models;
using JuanMVC.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace JuanMVC.Services
{
    public class LayoutService
    {
        private readonly JuanDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LayoutService(JuanDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Category> GetCategory()
        {
            return _context.Categories.ToList();
        }

        public List<Brand> GetBrand()
        {
            return _context.Brands.ToList();
        }

        public Dictionary<string,string> GetSetting()
        {
            return _context.Settings.ToDictionary(x => x.Key, x => x.Value);
        }


        public BasketVM GetBasket()
        {
            BasketVM vm = new BasketVM();


            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var items = _context.BasketItems.Include(x => x.Product).ThenInclude(x => x.Images.Where(x => x.ImageStatus == true)).Where(x => x.AppUserId == userId).ToList();

                foreach (var bi in items)
                {

                    BasketItemVM basketItemVM = new BasketItemVM()
                    {
                        Count = bi.Count,
                        Product = bi.Product
                    };

                    vm.Items.Add(basketItemVM);
                    vm.TotalAmount += (basketItemVM.Product.DiscountedPrice > 0 ? basketItemVM.Product.DiscountedPrice : basketItemVM.Product.SalePrice) * basketItemVM.Count;

                }

            }
            else
            {
                var basketStr = _httpContextAccessor.HttpContext.Request.Cookies["basket"];

                List<BasketCookieItemVM> cookieItems = null;

                if (basketStr != null)
                    cookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basketStr);
                else
                    cookieItems = new List<BasketCookieItemVM>();





                foreach (var cItem in cookieItems)
                {

                    BasketItemVM basketItemVM = new BasketItemVM()
                    {
                        Count = cItem.Count,
                        Product = _context.Products.Include(x => x.Images.Where(x => x.ImageStatus == true)).FirstOrDefault(x => x.Id == cItem.ProductId)
                    };

                    vm.Items.Add(basketItemVM);
                    vm.TotalAmount += (basketItemVM.Product.DiscountedPrice > 0 ? basketItemVM.Product.DiscountedPrice : basketItemVM.Product.SalePrice) * basketItemVM.Count;

                }
            }

            return vm;
        }

    }
}
