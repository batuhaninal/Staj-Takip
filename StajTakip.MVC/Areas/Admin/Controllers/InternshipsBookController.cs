using Microsoft.AspNetCore.Mvc;

namespace StajTakip.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InternshipsBookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
