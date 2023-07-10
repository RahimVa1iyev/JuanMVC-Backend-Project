using JuanMVC.DAL;
using JuanMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace JuanMVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly JuanDbContext _context;

        public BasketController(JuanDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var basketStr = Request.Cookies["basket"];

            List<BasketCookieItemVM> cookieItems = null;

            if(basketStr == null)
                cookieItems = new List<BasketCookieItemVM>();
            else
                cookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basketStr);

            BasketVM basketVM = new BasketVM();

            foreach (var item in cookieItems)
            {
                BasketItemVM basketItemVM = new BasketItemVM()
                {
                    Product = _context.Products.Include(x => x.Images.Where(x => x.ImageStatus == true)).FirstOrDefault(x => x.Id == item.ProductId),
                    Count = item.Count,
                };
                basketVM.Items.Add(basketItemVM);
                basketVM.TotalAmount += (basketItemVM.Product.DiscountedPrice > 0 ? basketItemVM.Product.DiscountedPrice : basketItemVM.Product.SalePrice) * basketItemVM.Count;


            }


            return View(basketVM);
        }

       


    }
}
