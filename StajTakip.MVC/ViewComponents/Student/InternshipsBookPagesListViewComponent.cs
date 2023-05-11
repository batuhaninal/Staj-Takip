using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.Business.Concrete;
using StajTakip.DataAccess.Concrete.EntityFramework.Repositories;

namespace StajTakip.MVC.ViewComponents.Student
{
    public class InternshipsBookPagesListViewComponent : ViewComponent
    {
        private readonly IInternshipsBookService _service;

        public InternshipsBookPagesListViewComponent(IInternshipsBookService service)
        {
            _service = service;
        }

        public IViewComponentResult Invoke()
        {
            var bookPages = _service.GetAll();

            return View(bookPages);
        }
    }
}
