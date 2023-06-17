using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;

namespace StajTakip.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        public IActionResult CheckSolved(int messageId)
        {
            var message = _messageService.GetByMessageId(messageId);
            if (!message.Success)
            {
                _notyfService.Error(message.Message ?? "Beklenmeyen bir hata meydana geldi! Lütfen daha sonra tekrar deneyiniz!");
                return RedirectToAction("Inbox");
            }
            message.Data.IsSolved = !message.Data.IsSolved;
            var result = _messageService.Update(message.Data);
            if (!result.Success)
            {
                _notyfService.Error(message.Message ?? "Beklenmeyen bir hata meydana geldi! Lütfen daha sonra tekrar deneyiniz!");
                return RedirectToAction("Inbox");
            }

            _notyfService.Success(message.Data.IsSolved ? "Sorun başarıyla çözüldü olarak işaretlendi!" : "Çözülmedi olarak işaretlendi!");
            return RedirectToAction("Inbox");
        }

        public IActionResult CheckRead(int messageId)
        {
            var result = _messageService.GetByMessageId(messageId);
            if (!result.Success)
            {
                _notyfService.Error(result.Message ?? "Beklenmeyen hata!");
                return RedirectToAction("Inbox");
            }

            result.Data.IsCompanyRead = User.IsInRole("admin.company") ? !result.Data.IsCompanyRead : result.Data.IsCompanyRead;
            result.Data.IsTeacherRead = User.IsInRole("admin.teacher") ? !result.Data.IsTeacherRead : result.Data.IsTeacherRead;
            var updateResult = _messageService.Update(result.Data);
            if (!updateResult.Success)
            {
                _notyfService.Error(updateResult.Message ?? "Beklenmeyen hata!");
                return RedirectToAction("Inbox");
            }

            if (User.IsInRole("admin.company"))
            {
                _notyfService.Success(result.Data.IsCompanyRead == false ? "Mesaj okunmadı olarak işaretlendi!" : "Mesaj okundu olarak işaretlendi!");
            } 
            else if (User.IsInRole("admin.teacher"))
            {
                _notyfService.Success(result.Data.IsTeacherRead == false ? "Mesaj okunmadı olarak işaretlendi!" : "Mesaj okundu olarak işaretlendi!");
            }
           
            return RedirectToAction("Inbox");
        }
    }
}
