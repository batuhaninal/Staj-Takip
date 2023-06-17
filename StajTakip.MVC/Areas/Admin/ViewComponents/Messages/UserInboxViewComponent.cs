using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;

namespace StajTakip.MVC.Areas.Admin.ViewComponents.Messages
{
    public class UserInboxViewComponent : ViewComponent
    {
        private readonly IMessageService _messageService;

        public UserInboxViewComponent(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public IViewComponentResult Invoke()
        {
            int adminUserId = int.Parse(User.Identity.Name);
            ViewBag.UnreadMessage = 0;
            if (User.IsInRole("admin.teacher"))
            {
                var messages = _messageService.GetLastNotificationListByUser(adminUserId);
                ViewBag.UnreadMessage = _messageService.GetInboxNewMessageCount(adminUserId, Entities.ComplexTypes.Roles.Teacher);
                return View(messages.Data);
            }else if (User.IsInRole("admin.company"))
            {
                var messages = _messageService.GetLastNotificationListByCompany(adminUserId);
                ViewBag.UnreadMessage = _messageService.GetInboxNewMessageCount(adminUserId, Entities.ComplexTypes.Roles.Company);
                return View(messages.Data);
            }      
            return View();
        }
    }
}
