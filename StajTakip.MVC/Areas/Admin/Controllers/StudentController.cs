using AspNetCoreHero.ToastNotification.Abstractions;
using Core.Utilities.Results;
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
        private readonly IAdminUserService _adminUserService;
        private readonly INotyfService _notyfService;

        public StudentController(IStudentUserService studentUserService, INotyfService notyfService, IAdminStudentRelationService adminStudentRelationService, IAdminUserService adminUserService)
        {
            _studentUserService = studentUserService;
            _notyfService = notyfService;
            _adminStudentRelationService = adminStudentRelationService;
            _adminUserService = adminUserService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Authorize(Roles ="admin.teacher, admin.company")]
        public IActionResult StudentList()
        {
            var data = _adminStudentRelationService.GetAllByAdminIdWithStudent(int.Parse(User.Identity.Name));
            return View(data.Data);
        }

        //[HttpGet]
        //[Authorize(Roles = "admin, admin.teacher")]
        //public IActionResult CompanyList()
        //{

        //}

        [HttpGet]
        [Authorize(Roles = "admin.teacher")]
        public IActionResult AllStudents()
        {
            var data = _studentUserService.GetAllWithEmail();
            return View(data.Data);
        }

        [Authorize(Roles = "admin.teacher")]
        public IActionResult AddStudentRelation(int studentId)
        {
            var model = new AdminStudentRelation
            {
                StudentUserId = studentId,
                AdminUserId = int.Parse(User.Identity.Name),
                IsCompany = User.IsInRole("admin.company")
            };
            Core.Utilities.Results.IResult result;
            if (model.IsCompany)
                result = _adminStudentRelationService.AddForCompany(model);
            else
                result = _adminStudentRelationService.AddForTeacher(model); 

            if (result.Success)
            {
                _notyfService.Success("Basariyla eklendi!");
                return RedirectToAction("AllStudents");
            }
            _notyfService.Error(result.Message ?? "Bir hatayla karşılaşıldı!");
            return RedirectToAction("AllStudents");
        }

        [Authorize(Roles = "admin.teacher")]
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
