﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.Entities.Concrete;

namespace StajTakip.MVC.Controllers
{
    public class InternshipDocumentController : Controller
    {
        private readonly IInternshipDocumentService _internshipDocumentService;
        private readonly ISignatureService _signatureService;
        private readonly INotyfService _notyfService;

        public InternshipDocumentController(IInternshipDocumentService internshipDocumentService, ISignatureService signatureService, INotyfService notyfService)
        {
            _internshipDocumentService = internshipDocumentService;
            _signatureService = signatureService;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            var documents = _internshipDocumentService.GetAll();
            ViewData["documentList"] = documents.Data;

            return View();
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
            var userId = User.Identity.Name;

            internshipDocument.StudentUserId = int.Parse(userId);
            if (pdfFile != null && pdfFile.Length > 0)
            {

                if (Path.GetExtension(pdfFile.FileName).ToLower() != ".pdf")
                {
                    ModelState.AddModelError("pdfFile", "Lütfen pdf belgesi yükleyiniz.");
                    return RedirectToAction("Index");
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

            return RedirectToAction("Index");
        }
        public IActionResult UploadSignature(string signature, int documentId)
        {
            var documentS = _internshipDocumentService.Get(documentId);
            var signatureSub = signature.Substring(22);  // remove data:image/png;base64,

            byte[] bytes = Convert.FromBase64String(signatureSub);
            if (documentS.Success)
            {
                using (MemoryStream pdfStream = new MemoryStream(documentS.Data.Data))
                {

                    Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(pdfStream);



                    // Set coordinates where image is to be pasted
                    int lwrLeftX = 370;
                    int lwrLeftY = 150;
                    int uprRightX = 220;
                    int uprRightY = 220;

                    // Access the target page to add an image
                    Aspose.Pdf.Page targetPage = pdfDocument.Pages[1];

                    var signatureImg = bytes;
                    // Load desired image into file stream
                    MemoryStream imgStream = new MemoryStream(signatureImg);

                    // Add the desired image to the images resource list of the target page
                    targetPage.Resources.Images.Add(imgStream);

                    // Save the current graphics state
                    targetPage.Contents.Add(new Aspose.Pdf.Operators.GSave());

                    // Create Rectangle and Matrix objects
                    Aspose.Pdf.Rectangle rect = new Aspose.Pdf.Rectangle(lwrLeftX, lwrLeftY, uprRightX, uprRightY);
                    Aspose.Pdf.Matrix mtrx = new Aspose.Pdf.Matrix(new double[] { rect.URX - rect.LLX, 0, 0, rect.URY - rect.LLY, rect.LLX, rect.LLY });

                    // Define how the image be placed in the PDF
                    targetPage.Contents.Add(new Aspose.Pdf.Operators.ConcatenateMatrix(mtrx));
                    Aspose.Pdf.XImage xImg = targetPage.Resources.Images[targetPage.Resources.Images.Count];

                    // Draw the image
                    targetPage.Contents.Add(new Aspose.Pdf.Operators.Do(xImg.Name));

                    // Restores the graphics state
                    targetPage.Contents.Add(new Aspose.Pdf.Operators.GRestore());

                    // Save updated document

                    using (MemoryStream updatedPdfStream = new MemoryStream())
                    {
                        pdfDocument.Save(updatedPdfStream);
                        var userId = User.Identity.Name;

                        documentS.Data.StudentUserId = int.Parse(userId);
                        documentS.Data.Data = updatedPdfStream.ToArray();
                        var result = _internshipDocumentService.Update(documentS.Data);
                        if (!result.Success)
                        {
                            _notyfService.Error(result.Message);
                        }
                        _notyfService.Success(result.Message);
                        return RedirectToAction("Index");


                    }
                }

            }
            return RedirectToAction("Index");
        }




    }
}