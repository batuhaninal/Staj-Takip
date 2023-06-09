using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;

namespace StajTakip.MVC.ViewComponents.Student
{
    public class StudentUserMenuViewComponent : ViewComponent
    {
        private readonly IStudentUserService _studentUserService;

        public StudentUserMenuViewComponent(IStudentUserService studentUserService)
        {
            _studentUserService = studentUserService;
        }

        public IViewComponentResult Invoke()
        {
            var user = _studentUserService.GetById(int.Parse(User.Identity.Name));
            return View(user.Data);
        }
    }
}
