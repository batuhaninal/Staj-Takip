using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;

namespace StajTakip.MVC.Controllers
{
    [Authorize(Roles ="student")]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly INotyfService _notyfService;
        public MessageController(IMessageService messageService, INotyfService notyfService)
        {
            _messageService = messageService;
            _notyfService = notyfService;
        }


        public IActionResult Inbox()
        {
            var messages = _messageService.GetInboxListByUser(int.Parse(User.Claims
                .Where(x => x.Type == "userId")
                .Select(x => x.Value)
                .FirstOrDefault()));
            if (messages.Success)
            {
                _notyfService.Information("Sayfa başarıyla yüklendi!");
                return View(messages.Data);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Sendbox()
        {
            var messages = _messageService.GetSendboxListByUser(int.Parse(User.Claims
                .Where(x => x.Type == "userId")
                .Select(x => x.Value)
                .FirstOrDefault()));
            if (messages.Success)
            {
                _notyfService.Information("Sayfa başarıyla yüklendi!");
                return View(messages.Data);
            }
            return View();
        }

        public IActionResult Delete(int messageId)
        {
            var result = _messageService.Delete(messageId);
            if (result.Success)
            {
                _notyfService.Success(result.Message ?? "Silme işlemi başarılı!");
                return RedirectToAction("Sendbox");
            }
            _notyfService.Error(result.Message ?? "Bir hata oluştu! Lütfen daha sonra tekrar deneyiniz!");
            return RedirectToAction("Sendbox");
        }

       
    }
}
