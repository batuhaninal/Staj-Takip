using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;

namespace StajTakip.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin, admin.teacher, admin.company")]
    public class InternshipDocumentController : Controller
    {
        private readonly IInternshipDocumentService _internshipDocumentService;
        private readonly INotyfService _notyfService;

        public InternshipDocumentController(IInternshipDocumentService internshipDocumentService, INotyfService notyfService)
        {
            _internshipDocumentService = internshipDocumentService;
            _notyfService = notyfService;
        }

        [HttpGet]
        [Authorize(Roles = "admin.teacher, admin.company")]
        public IActionResult Documents(int studentId)
        {
            var data = _internshipDocumentService.GetAllByStudentId(studentId);
            return View(data.Data);
        }

        public IActionResult CheckDocument(int documentId)
        {
            var document = _internshipDocumentService.Get(documentId);
            if (!document.Success)
            {
                _notyfService.Error(document.Message ?? "Hata!");
                return RedirectToAction("StudentList", "Student");
            }
            document.Data.IsTeacherChecked = User.IsInRole("admin.teacher") ? true : document.Data.IsTeacherChecked;
            document.Data.IsCompanyChecked = User.IsInRole("admin.company") ? true : document.Data.IsCompanyChecked;
            var result = _internshipDocumentService.Update(document.Data);
            if (!result.Success)
                _notyfService.Error(result.Message ?? "Hata!");
            _notyfService.Success("Evrak başarıyla onaylandı!");
            return RedirectToAction("Documents", new { studentId = document.Data.StudentUserId });
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

                    if (User.IsInRole("admin.teacher"))
                    {
                        lwrLeftX = 600;
                        lwrLeftY = 150;
                        uprRightX = 450;
                        uprRightY = 220;
                    }
                    else if (User.IsInRole("admin.company"))
                    {
                        lwrLeftX = 370;
                        lwrLeftY = 150;
                        uprRightX = 220;
                        uprRightY = 220;
                    }

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
                        documentS.Data.Data = updatedPdfStream.ToArray();
                        var result = _internshipDocumentService.Update(documentS.Data);
                        if (!result.Success)
                        {
                            _notyfService.Error(result.Message);
                        }
                        _notyfService.Success(result.Message);

                    }
                }
                return RedirectToAction("Documents", new { studentId = documentS.Data.StudentUserId });
            }
            _notyfService.Error("Beklenmeyen bir hata meydana geldi! Lütfen daha sonra tekrar deneyiniz.");
            return RedirectToAction("StudentList");
        }
    }
}
