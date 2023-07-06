using Microsoft.AspNetCore.Mvc;

namespace JuanMVC.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
