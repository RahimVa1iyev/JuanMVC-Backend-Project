using Microsoft.AspNetCore.Mvc;

namespace JuanMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {


            return View();
        }
    }
}
