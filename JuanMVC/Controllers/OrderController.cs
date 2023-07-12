using JuanMVC.DAL;
using JuanMVC.Enums;
using JuanMVC.Models;
using JuanMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace JuanMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly JuanDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(JuanDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Checkout()
        {
            CheckoutVM vm = new CheckoutVM();
            vm.Order = new OrderVM();


            var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            vm.Items = _getCheckOutItem(userId);
            vm.TotalAmount = vm.Items.Sum(x => x.Price);



            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);

                vm.Order.FullName = user.FullName;
                vm.Order.Phone = user.PhoneNumber;
                vm.Order.Email = user.Email;
            }
          

            return View(vm);
        }

        public async Task<IActionResult> Create(OrderVM orderVM)
        {
            OrderItem vm = new OrderItem();

            var userId = (User.Identity.IsAuthenticated && User.IsInRole("Member")) ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            var member =  (User.Identity.IsAuthenticated && User.IsInRole("Member")) ? await _userManager.FindByIdAsync(userId) : null;

            if (!ModelState.IsValid)
            {

                CheckoutVM viewmodel = new CheckoutVM();
                viewmodel.Order = orderVM;
                viewmodel.Items = _getCheckOutItem(userId);
                viewmodel.TotalAmount = viewmodel.Items.Sum(x => x.Price);
                return View("checkout", vm);
            }


            Order order = new Order()
            {
                FullName = member==null? orderVM.FullName : member.FullName,
                Email = member==null? orderVM.Email : member.Email,
                Phone = member==null? orderVM.Phone : member.PhoneNumber,
                Address = orderVM.Address,
                Text = orderVM.Text,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                Status = OrderStatus.Pending,
                UserId = userId

            };


            if (userId !=null)
            {
                var user = _userManager.FindByIdAsync(userId).Result;



                var basketItems = _context.BasketItems.Include(x => x.Product).Where(x => x.AppUserId == userId).ToList();

                order.OrderItems = basketItems.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Count = x.Count,
                    UnitSalePrice=x.Product.SalePrice,
                    UnitCostPrice = x.Product.CostPrice,
                    UnitDiscountedPrice = x.Product.DiscountedPrice,

                }).ToList();
                

            }
            else
            {
                var basketStr = Request.Cookies["basket"];

                List<BasketCookieItemVM> cookieItems = null;

                if (basketStr != null)
                    cookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basketStr);
                else
                    cookieItems = new List<BasketCookieItemVM>();

                foreach (var item in cookieItems)
                {
                    var product = _context.Products.Find(item.ProductId);

                    OrderItem orderItem = new OrderItem()
                    {
                        ProductId = item.ProductId,
                        Count = item.Count,
                        UnitSalePrice = product.SalePrice,
                        UnitCostPrice = product.CostPrice,
                        UnitDiscountedPrice = product.DiscountedPrice,
                    };

                    order.OrderItems.Add(orderItem);
                }
            }
            order.TotalAmount = order.OrderItems.Sum(x => x.Count * (x.UnitDiscountedPrice > 0 ? x.UnitDiscountedPrice : x.UnitSalePrice));


            _context.Orders.Add(order);
            _context.SaveChanges();

            _clearBasket(userId);


            if (userId != null )
            {
                return RedirectToAction("profile", "account", new { tab = "Orders" });

            }


            return RedirectToAction("index" , "home");
        }

        private List<CheckoutItemVM> _getCheckOutItem(string userId)
        {
            List<CheckoutItemVM> vm = new List<CheckoutItemVM>();

            if (userId != null)
            {
                var basketItems = _context.BasketItems.Include(x => x.Product).Where(x => x.AppUserId == userId).ToList();

                vm = basketItems.Select(x => new CheckoutItemVM
                {
                    ProductName = x.Product.Name,
                    Count = x.Count,
                    Price = x.Count * (x.Product.DiscountedPrice > 0 ? x.Product.DiscountedPrice : x.Product.SalePrice)

                }).ToList();

               
            }
            else
            {
                var basketStr = Request.Cookies["basket"];

                List<BasketCookieItemVM> cookieItems = null;

                if (basketStr != null)
                    cookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basketStr);
                else
                    cookieItems = new List<BasketCookieItemVM>();

                foreach (var item in cookieItems)
                {
                    var product = _context.Products.Find(item.ProductId);

                    vm.Add(new CheckoutItemVM
                    {
                        ProductName = product.Name,
                        Count = item.Count,
                        Price = item.Count * (product.DiscountedPrice > 0 ? product.DiscountedPrice : product.SalePrice)
                    });


                }

            }

            return vm;

        }

        private void _clearBasket(string userId)
        {
            if (userId == null)
                Response.Cookies.Delete("basket");
            else
            {
                _context.RemoveRange(_context.BasketItems.Where(x => x.AppUserId == userId));
                _context.SaveChanges();
            }
          
        }
    }
}
