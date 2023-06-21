using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.DataAccess.Migrations;
using StajTakip.Entities.Concrete;
using StajTakip.Entities.DTOs;
using System.Text.Json;

namespace StajTakip.MVC.Controllers
{
    [Authorize(Roles = "admin, student")]
    public class InternshipsBookController : Controller
    {
        private readonly IInternshipsBookService _bookRepository;
        private readonly IBookTemplateService _bookTemplateService;
        private readonly IBookImageService _bookImageService;
        private readonly IMessageService _messageService;
        private readonly INotyfService _notyfService;

        public InternshipsBookController(IInternshipsBookService bookRepository, INotyfService notifyService, IBookTemplateService bookTemplateService, IBookImageService bookImageService, IMessageService messageService)
        {
            _bookRepository = bookRepository;
            _notyfService = notifyService;
            _bookTemplateService = bookTemplateService;
            _bookImageService = bookImageService;
            _messageService = messageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var template = _bookTemplateService.GetCurrent();
            if (!template.Success || template.Data == null)
                ViewBag.Template = null;
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
            var template = _bookTemplateService.GetCurrent();
            ViewBag.Template = template.Data.Template;
            return View();
        }

        [HttpGet]
        public IActionResult Page(int pageId)
        {
            var page = _bookRepository.GetWithImages(pageId);
            if (page.Success)
            {
                ViewData["PageCount"] = _bookRepository.GetCount(page.Data.StudentUserId);
                _notyfService.Information("Sayfa başarıyla yüklendi!");
      
                return View(page.Data);
            }
            _notyfService.Error(page.Message);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Page(InternshipsBookPageUpdateDto model)
        {
            if (ModelState.IsValid)
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

        [HttpGet]
        public IActionResult AddImagePopup(int bookId)
        {
            ViewBag.bookId = bookId;
            return PartialView("AddImagePopup");
        }

        [HttpPost]
        public IActionResult AddImagePopup(BookImage model, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {

                //if (Path.GetExtension(image.FileName).ToLower() != ".jpg")
                //{
                //    ModelState.AddModelError("pdfFile", "Lütfen pdf belgesi yükleyiniz.");
                //    _notyfService.Error("Lütfen pdf belgesi yükleyiniz!");
                //    return RedirectToAction("Index");
                //}

                using (var memoryStream = new MemoryStream())
                {
                    image.CopyTo(memoryStream);
                    model.Data = memoryStream.ToArray();
                    var result = _bookImageService.Add(model);
                    var parsedJson = JsonSerializer.Serialize(result);
                    if (!result.Success)
                    {
                        //_notyfService.Error(result.Message);
                        return Json(parsedJson);
                    }
                    //_notyfService.Success(result.Message);
                    return Json(parsedJson);
                }
            }
            return Json("0");
        }

        public IActionResult ShowImage(int imageId)
        {
            var image = _bookImageService.Get(imageId);
            if (image.Success)
            {
                return File(image.Data.Data, "image/png");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ImageShowPopup(int imageId)
        {
            var image = _bookImageService.Get(imageId);
            if (image.Success)
            {
                var base64String = Convert.ToBase64String(image.Data.Data);
                string src = string.Format("data:image/png;base64,{0}", base64String);
                ViewBag.ImageUrl = src;
                return PartialView("ImageShowPopup", image.Data);
            }
            return PartialView("ImageShowPopup", new BookImage());
        }

        public bool DeleteImage(int imageId)
        {
            var image = _bookImageService.Get(imageId);
            var result = _bookImageService.HardDelete(image.Data);
            if (result.Success)
            {
                return true;
            }
            return false;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ImageList(int bookId)
        {
            return ViewComponent("ImageList", new {bookId=bookId});
        }

        public IActionResult BookPagePagination(int? currentPage)
        {
            return ViewComponent("InternshipsBookPagesList", currentPage);
        }

        public IActionResult TemplateIssue()
        {
            var result = _messageService.SendTemplateIssue(int.Parse(User.Identity.Name));
            if (!result.Success)
            {
                _notyfService.Error(result.Message ?? "Beklenmeyen bir hata meydana geldi! Lütfen daha sonra tekrar deneyiniz!");
                return RedirectToAction("Index");
            }
            _notyfService.Success("Sorun ile ilgili bilgilendirme danışmanınıza başarıyla gönderildi!");
            return RedirectToAction("Index");
        }
    }
}
