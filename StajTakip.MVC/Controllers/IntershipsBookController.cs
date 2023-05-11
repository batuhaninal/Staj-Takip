using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.Entities.DTOs;

namespace StajTakip.MVC.Controllers
{
    public class InternshipsBookController : Controller
    {
        private readonly IInternshipsBookService _bookRepository;
        private readonly INotyfService _notyfService;

        public InternshipsBookController(IInternshipsBookService bookRepository, INotyfService notifyService)
        {
            _bookRepository = bookRepository;
            _notyfService = notifyService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Page(int id)
        {
            var page = _bookRepository.Get(id);
            if (page.Success)
            {
                _notyfService.Success("Sayfa basariyla yuklendi!");
                return View(page.Data);
            }
            _notyfService.Error(page.Message);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Add(InternshipsBookPageAddDto model)
        {
            if (ModelState.IsValid)
            {
                var result = _bookRepository.Add(model);
                if (!result.Success)
                    _notyfService.Error(result.Message);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Update(InternshipsBookPageUpdateDto model)
        {
            if(ModelState.IsValid)
            {
                var result = _bookRepository.Update(model);
                if (!result.Success)
                {
                    _notyfService.Error(result.Message);
                    return RedirectToAction("Page", model.Id);
                }
            }
            return RedirectToAction("Page", model.Id);
        }
    }
}
