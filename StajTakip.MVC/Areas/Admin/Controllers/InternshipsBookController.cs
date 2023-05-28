using AspNetCoreHero.ToastNotification.Abstractions;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.Entities.Concrete;
using StajTakip.Entities.DTOs;

namespace StajTakip.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin,admin.teacher, admin.company")]
    public class InternshipsBookController : Controller
    {
        private readonly IInternshipsBookService _internshipsBookService;
        private readonly INotyfService _notyfService;

        public InternshipsBookController(IInternshipsBookService internshipsBookService, INotyfService notyfService)
        {
            _internshipsBookService = internshipsBookService;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BookPage(int studentId,int bookId)
        {
            var data = _internshipsBookService.Get(bookId);
            
            if (!data.Success)
            {
                ViewBag.studentId = studentId;
                return View();
            }
            else
            {
                ViewBag.studentId = data.Data.StudentUserId;
                return View(data.Data);
            }
                
        }

        [HttpPost]
        public IActionResult CheckBook(CheckBookDto model)
        {
            if (ModelState.IsValid)
            {
                model.IsTeacherChecked = User.IsInRole("admin.teacher") ? true : model.IsTeacherChecked;
                model.IsCompanyChecked = User.IsInRole("admin.company") ? true : model.IsCompanyChecked;
                var result = _internshipsBookService.CheckBook(model);
                if (result.Success)
                {
                    _notyfService.Success("Defter Kontrolu Basarili");
                    return RedirectToAction("BookPage", new { bookId = model.Id });
                }
            }
            _notyfService.Error("Lütfen daha sonra tekrar deneyiniz!");
            return RedirectToAction("BookPage", new { bookId = model.Id });
        }
    }
}
