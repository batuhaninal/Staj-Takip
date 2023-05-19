using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StajTakip.Business.Abstract;
using StajTakip.Entities.DTOs;

namespace StajTakip.MVC.Controllers
{
    [Authorize(Roles ="admin, student")]
    public class InternshipsBookController : Controller
    {
        private readonly IInternshipsBookService _bookRepository;
        private readonly IBookTemplateService _bookTemplateService;
        private readonly INotyfService _notyfService;

        public InternshipsBookController(IInternshipsBookService bookRepository, INotyfService notifyService, IBookTemplateService bookTemplateService)
        {
            _bookRepository = bookRepository;
            _notyfService = notifyService;
            _bookTemplateService = bookTemplateService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var template =_bookTemplateService.GetCurrent();
            if (!template.Success)
                ViewBag.Template = "";
            else
                ViewBag.Template = template.Data.Template;
            return View();
        }

        [HttpPost]
        public IActionResult Index(InternshipsBookPageAddDto model)
        {
            if (ModelState.IsValid)
            {
                var result = _bookRepository.Add(model);
                if (!result.Success)
                {
                    _notyfService.Error(result.Message);
                }
                return RedirectToAction("Index");
            }
            _notyfService.Error("Lütfen alanları kontrol ediniz!");
            return View();
        }

        [HttpGet]
        public IActionResult Page(int pageId)
        {
            var page = _bookRepository.Get(pageId);
            if (page.Success)
            {
                _notyfService.Information("Sayfa başarıyla yüklendi!");
                return View(page.Data);
            }
            _notyfService.Error(page.Message);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Page(InternshipsBookPageUpdateDto model)
        {
            if(ModelState.IsValid)
            {
                var result = _bookRepository.Update(model);
                if (!result.Success)
                {
                    _notyfService.Error(result.Message);
                    return RedirectToAction("Page", new { pageId = model.Id });
                }
                _notyfService.Success($"{model.Date.ToShortDateString()} tarihli sayfa başarıyla güncellendi!");
                return RedirectToAction("Page", new { pageId = model.Id });
            }
            _notyfService.Error("Lütfen alanları yeniden kontrol ediniz!");
            return View();
        }

        public IActionResult BookPagePagination(int? currentPage)
        {
            return ViewComponent("InternshipsBookPagesList", currentPage);
        }
    }
}
