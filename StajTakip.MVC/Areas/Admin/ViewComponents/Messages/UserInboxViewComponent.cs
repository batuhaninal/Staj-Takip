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
            if (User.IsInRole("admin.teacher"))
            {
                var messages = _messageService.GetLastNotificationListByUser(int.Parse(User.Identity.Name));
                return View(messages.Data);
            }else if (User.IsInRole("admin.company"))
            {
                var messages = _messageService.GetLastNotificationListByCompany(int.Parse(User.Identity.Name));
                return View(messages.Data);
            }
            return View();
        }
    }
}
