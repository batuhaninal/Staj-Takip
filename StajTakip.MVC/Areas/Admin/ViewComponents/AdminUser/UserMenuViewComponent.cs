using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;

namespace StajTakip.MVC.Areas.Admin.ViewComponents.AdminUser
{
    public class UserMenuViewComponent : ViewComponent
    {
        private readonly IAdminUserService _adminUserService;

        public UserMenuViewComponent(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        public IViewComponentResult Invoke()
        {
            var user = _adminUserService.GetByAdminUserId(int.Parse(User.Identity.Name));
            if (user.Success)
            {
                return View(user.Data);
            }
            ModelState.AddModelError("", user.Message);
            return View();
        }
    }
}
