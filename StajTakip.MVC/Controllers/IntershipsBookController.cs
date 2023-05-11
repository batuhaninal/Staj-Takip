using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.Entities.DTOs;

namespace StajTakip.MVC.Controllers
{
    public class InternshipsBookController : Controller
    {
        private readonly IInternshipsBookService _bookRepository;

        public InternshipsBookController(IInternshipsBookService bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(InternshipsBookPageAddDto model)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.Add(model);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Page(int id)
        {
            var page = _bookRepository.Get(id);
            if (page.Success)
            {
                return View(page.Data);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(InternshipsBookPageUpdateDto model)
        {
            if(ModelState.IsValid)
            {
                var result = _bookRepository.Update(model);
                if (result.Success)
                {
                    return RedirectToAction("Page",model.Id);
                }
            }
            return View();
        }
    }
}
