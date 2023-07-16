﻿using JuanMVC.Areas.Manage.ViewModels;
using JuanMVC.DAL;
using JuanMVC.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuanMVC.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    [Area("manage")]
    public class DashboardController : Controller
    {
        private readonly JuanDbContext _context;

        public DashboardController(JuanDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<string> list = new List<string>();
            List<int> priceList = new List<int>();


            for (int i = 0; i < 12; i++)
            {
                DateTime startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMonths(-i);
                DateTime endDate = startDate.AddMonths(1);

                list.Add(startDate.ToString("MMM"));
                priceList.Add((int)_context.Orders
                    .Where(x => x.Status == OrderStatus.Accepted && x.CreatedAt >= startDate && x.CreatedAt < endDate)
                    .Sum(x => x.TotalAmount));
            }





            DashboardIndexVM vm = new()
            {
                PieChartDatas = new PieChartVM
                {
                    AcceptedOrderCount = _context.Orders.Where(x => x.Status == OrderStatus.Accepted).Count(),
                    RejectedOrderCount = _context.Orders.Where(x => x.Status == OrderStatus.Rejected).Count(),
                    PendingOrderCount = _context.Orders.Where(x => x.Status == OrderStatus.Pending).Count(),

                },

                BarChartDatas = new BarChartVM
                {
                    Months = list,
                    Price = priceList
                }

            };

            return View(vm);
        }
    }
}
