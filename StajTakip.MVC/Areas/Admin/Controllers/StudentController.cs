using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using System.Data;

namespace StajTakip.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin,admin.teacher,admin.company")]
    public class StudentController : Controller
    {
        private readonly IStudentUserService _studentUserService;
        private readonly IAdminStudentRelationService _adminStudentRelationService;
        private readonly INotyfService _notyfService;

        public StudentController(IStudentUserService studentUserService, INotyfService notyfService, IAdminStudentRelationService adminStudentRelationService)
        {
            _studentUserService = studentUserService;
            _notyfService = notyfService;
            _adminStudentRelationService = adminStudentRelationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult StudentList()
        {
            var data = _adminStudentRelationService.GetAllByAdminIdWithStudent(int.Parse(User.Identity.Name));
            return View(data.Data);
        }

        //[HttpGet]
        //public IActionResult AllStudents() 
        //{
        //    var data = _adminStudentRelationService.GetAllWithUsers()
        //}
    }
}
