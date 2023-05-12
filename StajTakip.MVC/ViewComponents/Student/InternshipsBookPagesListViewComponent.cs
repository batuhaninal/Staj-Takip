using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.Business.Concrete;
using StajTakip.DataAccess.Concrete.EntityFramework.Repositories;
using X.PagedList;

namespace StajTakip.MVC.ViewComponents.Student
{
    public class InternshipsBookPagesListViewComponent : ViewComponent
    {
        private readonly IInternshipsBookService _service;

        public InternshipsBookPagesListViewComponent(IInternshipsBookService service)
        {
            _service = service;
        }

        //public IViewComponentResult Invoke(int currentPage = 1)
        //{
        //    var bookPages = _service.GetPages().Data.ToPagedList(currentPage, 5);

        //    return View(bookPages);
        //}

        public IViewComponentResult Invoke()
        {
            var bookPages = _service.GetPages();

            return View(bookPages);
        }
    }
}
