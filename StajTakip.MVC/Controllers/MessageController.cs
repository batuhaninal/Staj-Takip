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

        [HttpGet]
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

        public IActionResult CheckRead(int messageId, int pageId)
        {
            var result = _messageService.GetByMessageId(messageId);
            if (!result.Success)
            {
                _notyfService.Error(result.Message ?? "Beklenmeyen hata!");
                return pageId == 1 ? RedirectToAction("Inbox") : RedirectToAction("Sendbox");
            }

            result.Data.IsStudentRead = !result.Data.IsStudentRead;
            var updateResult = _messageService.Update(result.Data);
            if(!updateResult.Success)
            {
                _notyfService.Error(updateResult.Message ?? "Beklenmeyen hata!");
                return pageId == 1 ? RedirectToAction("Inbox") : RedirectToAction("Sendbox");
            }

            _notyfService.Success(result.Data.IsStudentRead == false ? "Mesaj okunmadı olarak işaretlendi!" : "Mesaj okundu olarak işaretlendi!");
            return pageId == 1 ? RedirectToAction("Inbox") : RedirectToAction("Sendbox");
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

        public IActionResult Finish(int studentId) 
        {
            var result = _messageService.SendFinish(studentId);
            if(!result.Success)
            {
                _notyfService.Error(result.Message);
                return RedirectToAction("Sendbox");
            }
            _notyfService.Success(result.Message ?? "Bitirme isteği gönderildi!");
            return RedirectToAction("Sendbox");
        }

       
    }
}
