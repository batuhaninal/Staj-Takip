using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;

namespace StajTakip.MVC.Controllers
{
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
            var messages = _messageService.GetInboxListByUser(int.Parse(User.Identity.Name));
            if (messages.Success)
            {
                _notyfService.Success("Başarıyla yüklendi!");
                return View(messages.Data);
            } 
            return View();
        }

        [HttpGet]
        public IActionResult Sendbox()
        {
            var messages = _messageService.GetSendboxListByUser(int.Parse(User.Identity.Name));
            if (messages.Success)
            {
                _notyfService.Success("Başarıyla yüklendi!");
                return View(messages.Data);
            }
            return View();
        }


        //[HttpGet]
        //public IActionResult MessageDetails(int messageId)
        //{
        //    var message = _messageService.GetByMessageId(messageId);
        //    if(message.Success)
        //    {
        //        _notyfService.Success("Başarıyla yüklendi!");
        //        return View(message.Data);
        //    }
        //    _notyfService.Error(message.Message);
        //    return RedirectToAction("Index", "Home");
        //}
    }
}
