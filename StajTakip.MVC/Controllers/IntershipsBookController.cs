using Microsoft.AspNetCore.Mvc;

namespace StajTakip.MVC.Controllers
{
    public class InternshipsBookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
