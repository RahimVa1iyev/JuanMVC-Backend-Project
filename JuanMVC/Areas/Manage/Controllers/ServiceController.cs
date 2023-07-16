using JuanMVC.Areas.Manage.ViewModels;
using JuanMVC.DAL;
using JuanMVC.Helpers;
using JuanMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuanMVC.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    [Area("manage")]
    public class ServiceController : Controller
    {
        private readonly JuanDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ServiceController(JuanDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page =1 )
        {
            var query = _context.Services.AsQueryable();

            return View(PaginatedList<Service>.Create(query,page,5));
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Service service)
        {
            if (!ModelState.IsValid) return View();

            if (service.ImageFile == null)
            {
                ModelState.AddModelError("", "Field is reuired");
                return View();
            }

            service.IconImage = FileManager.Save(service.ImageFile, _env.WebRootPath, "manage/assets/uploads/icons");

            _context.Services.Add(service);
            _context.SaveChanges();

            return RedirectToAction("index");

        }

        public IActionResult Edit(int id)
        {
            var existService = _context.Services.Find(id);
            if (existService == null) return View("Error");


            return View(existService);
        }
        [HttpPost]

        public IActionResult Edit(Service service)
        {
            if (!ModelState.IsValid) return View();

            var existService = _context.Services.Find(service.Id);

             string removableImage = null;

            if(existService.ImageFile != null)
            {
                removableImage = existService.IconImage;
                existService.IconImage = FileManager.Save(existService.ImageFile, _env.WebRootPath, "manage/assets/uploads/icons");

            }

            existService.FirstTitle = service.FirstTitle;
            existService.SecondTitle = service.SecondTitle;
            existService.BackgroundColor = service.BackgroundColor;

            _context.SaveChanges();

            if (removableImage != null) FileManager.Delete(_env.WebRootPath, "manage/assets/uploads/icons", removableImage);


            return RedirectToAction("index");
        }

        
        public IActionResult Delete(int id)
        {
            var deletedProduct = _context.Services.Find(id);

            if(deletedProduct == null) return View("Error");

            var removableImage = deletedProduct.IconImage;

            _context.Services.Remove(deletedProduct);
            _context.SaveChanges();


            FileManager.Delete(_env.WebRootPath,"manage/assets/uploads/icons",removableImage);
            

            return RedirectToAction("index");
        }

    }
}
