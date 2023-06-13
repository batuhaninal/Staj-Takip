using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;

namespace StajTakip.MVC.ViewComponents.Student
{
    public class ImageListViewComponent : ViewComponent
    {
        private readonly IBookImageService _bookImageService;

        public ImageListViewComponent(IBookImageService bookImageService)
        {
            _bookImageService = bookImageService;
        }

        public IViewComponentResult Invoke(int bookId)
        {
            var images = _bookImageService.GetAllByBookId(bookId);
            if (images.Success)
                return View(images.Data);
            return View();
        }
    }
}
