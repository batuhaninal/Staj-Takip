using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;

namespace StajTakip.MVC.Areas.Admin.ViewComponents.InternshipsBooks
{
    public class InternshipsBookPagesViewComponent : ViewComponent
    {
        private readonly IInternshipsBookService _internService;

        public InternshipsBookPagesViewComponent(IInternshipsBookService internService)
        {
            _internService = internService;
        }

        public IViewComponentResult Invoke(int studentId)
        {
            var data = _internService.GetPageListDtoByStudentId(studentId);
            return View(data);
        }
    }
}
