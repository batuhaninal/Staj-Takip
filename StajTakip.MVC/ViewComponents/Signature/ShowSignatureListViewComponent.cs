using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;

namespace StajTakip.MVC.ViewComponents.Signature
{
    public class ShowSignatureListViewComponent : ViewComponent
    {
        private readonly ISignatureService _signatureService;

        public ShowSignatureListViewComponent(ISignatureService signatureService)
        {
            _signatureService = signatureService;
        }

        public IViewComponentResult Invoke()
        {
            var signatureImg = _signatureService.GetAll();
            return View(signatureImg);
        }
    }
}
