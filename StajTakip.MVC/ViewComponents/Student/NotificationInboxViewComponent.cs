using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.DataAccess.Abstract;

namespace StajTakip.MVC.ViewComponents.Student
{
    public class NotificationInboxViewComponent : ViewComponent
    {
        private readonly IMessageService _messageService;

        public NotificationInboxViewComponent(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public IViewComponentResult Invoke()
        {
            if (User.IsInRole("student"))
                ViewBag.UnreadMessage = _messageService.GetInboxNewMessageCount(int.Parse(User.Identity.Name), Entities.ComplexTypes.Roles.Student);
            else
                ViewBag.UnreadMessage = 0;

            return View();
        }
    }
}
