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
    public class SliderController : Controller
    {
        private readonly JuanDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(JuanDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page =1 ,string search =null)
        {
            ViewBag.Search = search;

            var query = _context.Sliders.AsQueryable();

            if (search != null)
            {
                query = query.Where(x => x.FirstTitle.Contains(search));
            }

            return View(PaginatedList<Slider>.Create(query,page,5));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("Image", "Image file is required");
                return View();
            }

            

            slider.Image = FileManager.Save(slider.ImageFile,_env.WebRootPath,"manage/assets/uploads/sliders");

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            var existSlider = _context.Sliders.Find(id);

            if (existSlider == null) return View("Error");

          
            return View(existSlider);
        }

        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            var existSlider = _context.Sliders.Find(slider.Id);

            if (existSlider == null) return View("Error");

            string removableImage = null;

            if (slider.ImageFile != null)
            {
                removableImage = slider.ImageFile.FileName;

                slider.Image = FileManager.Save(slider.ImageFile, _env.WebRootPath, "manage/assets/uploads/sliders");

            }

            _context.SaveChanges();

            if (removableImage != null) FileManager.Delete(_env.WebRootPath, "manage/assets/uploads/sliders", removableImage);
                    

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            var existSlider = _context.Sliders.Find(id);

            if (existSlider == null) return View("Error");

            var removableImage = existSlider.Image;

            _context.Sliders.Remove(existSlider);

            _context.SaveChanges();

            FileManager.Delete(_env.WebRootPath, "manage/assets/uploads/sliders", removableImage);

            return RedirectToAction("index");
        }
    }
}
