
using JuanMVC.DAL;
using JuanMVC.Models;
using JuanMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace JuanMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly JuanDbContext _context;

        public ProductController(JuanDbContext context)
        {
           

            _context = context;
        }

        public IActionResult Show(int id)
        {
            var existPr = _context.Products.Include(x => x.Images).Include(x => x.Brand).Include(x => x.Category).FirstOrDefault(x => x.Id == id);

            if (existPr == null) return View("index", "home");         

            return PartialView("_ModalPartial",existPr);
        }

        public IActionResult Basket(int id)
        {
            BasketVM vm = new BasketVM();


            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var basketItems = _context.BasketItems.Where(x => x.AppUserId == userId).ToList();

                var basketItem = basketItems.FirstOrDefault(x => x.ProductId == id);

                if (basketItem == null)
                {
                    basketItem = new()
                    {
                        Count = 1,
                        ProductId = id,
                        AppUserId = userId
                    };
                    _context.BasketItems.Add(basketItem);

                }
                else
                {
                    basketItem.Count++;
                }
                _context.SaveChanges();

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
                var basketStr = HttpContext.Request.Cookies["basket"];

                List<BasketCookieItemVM> cookieItems = null;

                if (basketStr != null)
                    cookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basketStr);
                else
                    cookieItems = new List<BasketCookieItemVM>();

                var existItem = cookieItems.FirstOrDefault(x => x.ProductId == id);

                if (existItem == null)
                {
                    var cookieItem = new BasketCookieItemVM()
                    {
                        ProductId = id,
                        Count = 1
                    };

                    cookieItems.Add(cookieItem);
                }
                else
                {
                    existItem.Count++;
                }

                HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(cookieItems));


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


            return PartialView("_BasketPartial",vm);
        }

        public IActionResult DeleteBasket(int id)
        {
            BasketVM vm = new BasketVM();
            var userId = User.Identity.IsAuthenticated? User.FindFirstValue(ClaimTypes.NameIdentifier): null;

            if ( userId !=null )
            {

                var basketItem = _context.BasketItems.FirstOrDefault(x => x.AppUserId==userId && x.ProductId == id);

                if (basketItem == null)
                    return View("Error");
                else
                {
                    _context.BasketItems.Remove(basketItem);
                    _context.SaveChanges();
                }

                var basketItems = _context.BasketItems.Include(x => x.Product).ThenInclude(p => p.Images).Where(x => x.AppUserId == userId).ToList();


                foreach (var item in basketItems)
                {

                    BasketItemVM basketItemVM = new BasketItemVM
                    {
                        Count = item.Count,
                        Product = item.Product

                    };

                    vm.Items.Add(basketItemVM);

                    vm.TotalAmount += (basketItemVM.Product.DiscountedPrice > 0 ? basketItemVM.Product.DiscountedPrice : basketItemVM.Product.SalePrice) * basketItemVM.Count;

                }


            }
            else
            {

                var basketStr = Request.Cookies["basket"];

                List<BasketCookieItemVM> cookieItems = null;

                if (basketStr == null)
                    cookieItems = new List<BasketCookieItemVM>();
                else
                    cookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basketStr);

                var existItem = cookieItems.FirstOrDefault(x => x.ProductId == id);

                if (existItem == null)
                    return View("Error");
                else
                    cookieItems.Remove(existItem);

                Response.Cookies.Append("basket", JsonConvert.SerializeObject(cookieItems));

               

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



            return PartialView("_BasketPartial",vm);
        }

        public IActionResult Detail(int id)
        {
           var vm = _getProductDetail(id);

            if (vm.Product == null) return View("Error");

          
            return View(vm);
        }

        [Authorize(Roles ="Member")]
        [HttpPost]
        public IActionResult Review(ProductReview review)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (!ModelState.IsValid)
            {
                var vm = _getProductDetail(review.ProductId);


                vm.Review = review;

                return View("Detail",vm);
            }

            var pr = _context.ProductReviews.FirstOrDefault(x => x.AppUserId == userId && x.ProductId==review.ProductId);

            if (pr != null)
            {
                ModelState.AddModelError("", "Almost you have comment to this shoe");

                var vm = _getProductDetail(review.ProductId);


                vm.Review = review;

                return View("Detail", vm);

            }

            var product = _context.Products.Include(x=>x.ProductReviews).FirstOrDefault(x=>x.Id == review.ProductId);

            if (product == null) return View("Error");

            review.AppUserId = userId;
            review.CreatedAt = DateTime.UtcNow.AddHours(4);

            product.ProductReviews.Add(review);

            product.Rate =(byte)Math.Ceiling(product.ProductReviews.Average(x => x.Rate));

            _context.SaveChanges();


            return RedirectToAction("detail", new { id =review.ProductId });
        }

        private ProductDetailVM _getProductDetail(int id)
        {
            var product = _context.Products
              .Include(x => x.Category)
              .Include(x=>x.ProductReviews)
              .ThenInclude(x=>x.AppUser)
              .Include(x => x.Brand)
              .Include(x=>x.ProductSizes)
              .ThenInclude(x=>x.Size)
              .Include(x => x.Color)
              .Include(x => x.Images).FirstOrDefault(x => x.Id == id);


            ProductDetailVM vm = new ProductDetailVM()
            {
                Product = product,
                RelatedProducts = product != null ? _context.Products.Include(x => x.Images.Where(x => x.ImageStatus == true)).Where(x => x.BrandId == product.Id).Take(5).ToList(): null,
                Review = new ProductReview { ProductId = id }
            };

            return vm;
        }
    }
}
