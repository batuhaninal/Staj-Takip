using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.Entities.Concrete;

namespace StajTakip.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InternshipsBookController : Controller
    {
        private readonly IBookTemplateService _bookTemplateService;
        private readonly INotyfService _notyfService;

        public InternshipsBookController(IBookTemplateService bookTemplateService, INotyfService notyfService)
        {
            _bookTemplateService = bookTemplateService;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Template()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Template(BookTemplate model)
        {
            if (ModelState.IsValid)
            {
                var result = _bookTemplateService.Add(model);
                if (result.Success)
                {
                    _notyfService.Information("Template başarıyla eklendi!");
                    return RedirectToAction("Template");
                }
                _notyfService.Error("Bir hatayla karşılaşıldı. Lütfen daha sonra tekrar deneyiniz!");
                return RedirectToAction("Template");
            }
            _notyfService.Error("Lütfen alanları kontrol ediniz!");
            return View();
        }

        [HttpGet]
        public IActionResult TemplatePage(int templateId)
        {
            var template = _bookTemplateService.Get(templateId);
            if (template.Success)
            {
                _notyfService.Information("Sayfa başarıyla yüklendi");
                return View(template.Data);
            }
            _notyfService.Error("Verilen parametrede bir template bulunamadı!");
            return RedirectToAction("Template");
        }

        [HttpPost]
        public IActionResult TemplatePage(BookTemplate model)
        {
            if (ModelState.IsValid)
            {
                var result = _bookTemplateService.Update(model);
                if(result.Success)
                {
                    _notyfService.Success("Güncelleme işlemi başarılı!");
                }
                return RedirectToAction("TemplatePage", new { templateId = model.Id });
            }
            _notyfService.Error("Lütfen alanları yeniden kontrol ediniz!");
            return View();
        }
    }
}
