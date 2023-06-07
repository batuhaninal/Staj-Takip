using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.Entities.Concrete;
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

        [HttpGet]
        public IActionResult AllStudents()
        {
            var data = _studentUserService.GetAll();
            return View(data.Data);
        }

        public IActionResult AddStudentRelation(int studentId)
        {
            var model = new AdminStudentRelation
            {
                StudentUserId = studentId,
                AdminUserId = int.Parse(User.Identity.Name),
                IsCompany = User.IsInRole("admin.company")
            };
            var result = _adminStudentRelationService.Add(model);
            if (result.Success)
            {
                _notyfService.Success("Basariyla eklendi!");
                return RedirectToAction("AllStudents");
            }
            _notyfService.Error(result.Message ?? "Bir hatayla karşılaşıldı!");
            return RedirectToAction("AllStudents");
        }

        public IActionResult DeleteStudentRelation(int relationId)
        {
            var result = _adminStudentRelationService.HardDelete(relationId);
            if (result.Success)
            {
                _notyfService.Success("Başarıyla öğrenci listenizden silindi!");
                return RedirectToAction("StudentList");
            }
            _notyfService.Error(result.Message ?? "Bir hatayla karşılaşıldı!");
            return RedirectToAction("StudentList");
        }
    }
}
