using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StajTakip.Business.Abstract;
using StajTakip.DataAccess.Abstract;
using StajTakip.Entities.Concrete;

namespace StajTakip.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IAdminUserService _adminUserService;
        private readonly IAdminStudentRelationService _adminStudentRelationService;
        private readonly IStudentUserService _studentUserService;
        private readonly INotyfService _notyfService;

        public CompanyController(IAdminUserService adminUserService, IAdminStudentRelationService adminStudentRelationService, INotyfService notyfService, IStudentUserService studentUserService)
        {
            _adminUserService = adminUserService;
            _adminStudentRelationService = adminStudentRelationService;
            _notyfService = notyfService;
            _studentUserService = studentUserService;
        }

        [HttpGet]
        public IActionResult AllCompanyList()
        {
            var companies = _adminUserService.GetAllAdminWithRelations();
            return View(companies.Data);
        }

        [HttpGet]
        public IActionResult StudentListModal(int companyId)
        {
            var model = _studentUserService.GetAll();
            ViewBag.CompanyId = companyId;
            return PartialView("_AddCompanyRelationPartial", model.Data);
        }

        public IActionResult AddStudentForCompany(AdminStudentRelation postModel)
        {
            if (ModelState.IsValid)
            {
                var result = _adminStudentRelationService.Add(postModel);
                if(!result.Success)
                {
                    _notyfService.Error(result.Message ?? "Hata!");
                    return RedirectToAction("AllCompanyList");
                }
            }
            _notyfService.Success("Öğrenci başarılı bir şekilde şirkete eklendi!");
            return RedirectToAction("AllCompanyList");
        }

        [Authorize(Roles = "admin.teacher")]
        public IActionResult DeleteStudentRelation(int relationId)
        {
            var result = _adminStudentRelationService.HardDelete(relationId);
            if (result.Success)
            {
                _notyfService.Success("Öğrenci başarıyla şirket listesinden silindi!");
                return RedirectToAction("AllCompanyList");
            }
            _notyfService.Error(result.Message ?? "Bir hatayla karşılaşıldı!");
            return RedirectToAction("AllCompanyList");
        }
    }
}
