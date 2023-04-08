using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.Entities.Concrete;

namespace StajTakip.MVC.Controllers
{
    public class TempController : Controller
    {
        private readonly ITempService _tempService;

        public TempController(ITempService tempService)
        {
            _tempService = tempService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("add")]
        public IActionResult Add(Temp model)
        {
            _tempService.Add(model);
            return RedirectToAction("Index");
        }


        // addasync urlde görmüyor async kısmını okumuyor
        [HttpPost("addasync")]
        public async Task<IActionResult> AddAsync(Temp model) 
        {
            await _tempService.AddAsync(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var model = _tempService.GetById(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(Temp model) 
        { 
            _tempService.Update(model);
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            _tempService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _tempService.GetById(id);
            return View("Index",model);
        }
    }
}
