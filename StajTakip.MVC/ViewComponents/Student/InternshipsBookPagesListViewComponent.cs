using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.Business.Concrete;
using StajTakip.DataAccess.Concrete.EntityFramework.Repositories;
using StajTakip.Entities.DTOs;
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
            if(!User.Identity.IsAuthenticated)
            {
                return View(new ErrorDataResult<List<InternshipsBookPageListDto>>());
            }
            var userId = User.Identity.Name;
            var bookPages = _service.GetPagesByStudentId(int.Parse(userId));

            return View(bookPages);

        }
    }
}
