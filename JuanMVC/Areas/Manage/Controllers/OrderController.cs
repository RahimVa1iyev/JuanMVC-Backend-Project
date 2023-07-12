using JuanMVC.Areas.Manage.ViewModels;
using JuanMVC.DAL;
using JuanMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuanMVC.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]

    [Area("manage")]
    public class OrderController : Controller
    {
        private readonly JuanDbContext _context;

        public OrderController(JuanDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page=1)
        {
            var query = _context.Orders.Include(x => x.OrderItems).AsQueryable();


            return View(PaginatedList<Order>.Create(query,page,5));
        }

        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Include(_x => _x.OrderItems).ThenInclude(x=>x.Product).FirstOrDefault(x=>x.Id == id);

            if (order == null) return View("Error");

            return View(order);
        }

        public IActionResult Accept(int id)
        {
            var order = _context.Orders.Include(_x => _x.OrderItems).ThenInclude(x=>x.Product).FirstOrDefault(x=>x.Id == id);

            if (order == null) return View("Error");

            order.Status = Enums.OrderStatus.Accepted;

            _context.SaveChanges();



            return RedirectToAction("index");
        }

        public IActionResult Reject(int id)
        {
            var order = _context.Orders.Include(_x => _x.OrderItems).ThenInclude(x => x.Product).FirstOrDefault(x => x.Id == id);

            if (order == null) return View("Error");

            order.Status = Enums.OrderStatus.Rejected;

            _context.SaveChanges();



            return RedirectToAction("index");
        }


    }
}
