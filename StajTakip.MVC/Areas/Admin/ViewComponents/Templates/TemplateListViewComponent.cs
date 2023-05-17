using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;

namespace StajTakip.MVC.Areas.Admin.ViewComponents.Templates
{
    public class TemplateListViewComponent : ViewComponent
    {
        private readonly IBookTemplateService _bookTemplate;

        public TemplateListViewComponent(IBookTemplateService bookTemplate)
        {
            _bookTemplate = bookTemplate;
        }

        public IViewComponentResult Invoke()
        {
            var templates = _bookTemplate.GetAll();
            return View(templates);
        }
    }
}
