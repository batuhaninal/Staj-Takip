using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using System.Data;

namespace StajTakip.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin,admin.teacher")]
    public class StudentController : Controller
    {
        private readonly IStudentUserService _studentUserService;
        private readonly INotyfService _notyfService;

        public StudentController(IStudentUserService studentUserService, INotyfService notyfService)
        {
            _studentUserService = studentUserService;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult StudentList()
        {
            var data = _studentUserService.GetAll();
            return View(data);
        }
    }
}
