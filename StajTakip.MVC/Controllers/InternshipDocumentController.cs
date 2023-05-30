using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.Entities.Concrete;

namespace StajTakip.MVC.Controllers
{
    public class InternshipDocumentController : Controller
    {
        private readonly IInternshipDocumentService _internshipDocumentService;
        private readonly INotyfService _notyfService;

        public InternshipDocumentController(IInternshipDocumentService internshipDocumentService, INotyfService notyfService)
        {
            _internshipDocumentService = internshipDocumentService;
            _notyfService = notyfService;
        }



        //[HttpPost]
        //public IActionResult Index(InternshipDocument internshipDocument, IFormFile pdfFile)
        //{
        //    if (pdfFile != null && pdfFile.Length > 0)
        //    {
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            pdfFile.CopyTo(memoryStream);
        //            internshipDocument.Data = memoryStream.ToArray();
        //            var result = _internshipDocumentService.Add(internshipDocument);
        //            if (!result.Success)
        //            {
        //                _notyfService.Error(result.Message);
        //            }
        //            _notyfService.Success(result.Message);
        //            return RedirectToAction("Index");


        //        }
        //    }
        //    return View();
        //}

        public IActionResult Index()
        {
            var documents = _internshipDocumentService.GetAll().Data.FirstOrDefault();
            return View(documents);
        }

        [HttpGet]
        public IActionResult ShowPdf(int documentId)
        {
            var document = _internshipDocumentService.Get(documentId);

            if (document != null && document.Data != null)
            {
                return File(document.Data.Data, "application/pdf");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult UploadPdf(InternshipDocument internshipDocument, IFormFile pdfFile)
        {
            if (pdfFile != null && pdfFile.Length > 0)
            {
                // Validate if the uploaded file is a PDF
                if (Path.GetExtension(pdfFile.FileName).ToLower() != ".pdf")
                {
                    ModelState.AddModelError("pdfFile", "Please upload a PDF file.");
                    return View();
                }

                using (var memoryStream = new MemoryStream())
                {
                    pdfFile.CopyTo(memoryStream);
                    internshipDocument.Data = memoryStream.ToArray();
                    var result = _internshipDocumentService.Add(internshipDocument);
                    if (!result.Success)
                    {
                        _notyfService.Error(result.Message);
                    }
                    _notyfService.Success(result.Message);
                    return RedirectToAction("Index");
                }
            }

            return View();
        }


    }
}
