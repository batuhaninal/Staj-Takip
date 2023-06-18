using Core.Entities.Concrete;
using StajTakip.Entities.Concrete;
using StajTakip.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Constants
{
    public static class Messages
    {
        public static string IsExist = "Verilen parametreye ait bir kayit bulunamadi!";

        public static Message TempIssue()
        {
            return new Message
            {
                MessageDate = DateTime.Now,
                Subject = "Geçerli Template Hatası",
                MessageDetail = "Lütfen geçerli bir template oluşturun veya atayınız!",
                IsSolved = false,
                IsTeacherRead = false,
                IsCompanyRead = false,
                IsStudentRead = false
            };
        }

        public static Message StudentAddedDocument(string userFirstLastName, int documentId, string documentName)
        {
            return new Message
            {
                MessageDate = DateTime.Now,
                Subject = $"{userFirstLastName} öğrencisi {documentId} id değerine sahip {documentName} isimli yeni döküman ekledi!",
                MessageDetail = $"Lütfen eklenen {documentId} id değerine sahip dökümanı inceleyip gerekli işlemleri yapınız!",
                IsSolved = false,
                IsTeacherRead = false,
                IsCompanyRead = false,
                IsStudentRead = false
            };
        }

        public static Message SendFinish(int studentId,string userFirstLastName)
        {
            return new Message
            {
                MessageDate = DateTime.Now,
                Subject = $"{userFirstLastName} öğrencisi stajını bitirme isteği gönderdi!",
                MessageDetail = $"{userFirstLastName} isimli öğrenci stajını bitirme isteği gönderdi! Lütfen  {studentId} id değerine sahip öğrencinin dökümanlarını inceleyip öğrencilerim listesinden kabul ediniz!",
                IsSolved = false,
                IsTeacherRead = false,
                IsCompanyRead = false,
                IsStudentRead = false
            };
        }

        public static Message AcceptFinish( string studentFirstLastName, string adminFirstLastName)
        {
            return new Message
            {
                MessageDate = DateTime.Now,
                Subject = $"Tebrikler!",
                MessageDetail = $"{studentFirstLastName} isimli öğrencinin staj bitirme isteği {adminFirstLastName} tarafından kabul edildi! Stajınız başarıyla tamamlamış bulunmaktasınız!",
                IsSolved = false,
                IsTeacherRead = false,
                IsCompanyRead = false,
                IsStudentRead = false
            };
        }
        public static Message RejectFinish(string studentFirstLastName, string adminFirstLastName)
        {
            return new Message
            {
                MessageDate = DateTime.Now,
                Subject = $"{studentFirstLastName} öğrencisinin staj bitirme isteği reddedildi!",
                MessageDetail = $"{studentFirstLastName} isimli öğrencinin staj bitirme isteği {adminFirstLastName} tarafından ret edildi ! Lütfen  dökümanlarınızı inceleyip danışmanınız ile irtibata geçiniz!",
                IsSolved = false,
                IsTeacherRead = false,
                IsCompanyRead = false,
                IsStudentRead = false
            };
        }


        public static Message RejectDocument(string studentFirstLastName, string adminFirstLastName, int documentId, string documentName)
        {
            return new Message
            {
                MessageDate = DateTime.Now,
                Subject = $"{documentName} adlı dökümanınız ret edildi!",
                MessageDetail = $"<label>{studentFirstLastName} öğrencisi tarafından eklenen {documentId} id değerine sahip {documentName} isimli yeni döküman {adminFirstLastName} tarafından ret edildi!</label> <strong>Dökümanı silebilirsiniz!</strong>",
                IsSolved = true,
                IsTeacherRead = false,
                IsCompanyRead = false,
                IsStudentRead = false
            };
        }

        public static Message AcceptDocument(string adminFirstLastName, int documentId, string documentName)
        {
            return new Message
            {
                MessageDate = DateTime.Now,
                Subject = $"{documentName} adlı dökümanınız kabul edildi!",
                MessageDetail = $"{documentName} isimli {documentId} id değerine sahip dökümanınız {adminFirstLastName} tarafından kabul edildi! Belgelerim kısmından kontrol ediniz!",
                IsSolved = true,
                IsTeacherRead = false,
                IsCompanyRead = false,
                IsStudentRead = false
            };
        }


        public static Message SignedDocument(string userFirstLastName, int documentId, string documentName)
        {
            return new Message
            {
                MessageDate = DateTime.Now,
                Subject = $"{documentName} adlı dökümanınıza imzalama işlemi gerçekleştirildi!",
                MessageDetail = $"{documentName} isimli {documentId} id değerine sahip dökümanınız {userFirstLastName} tarafından imzalandı! Belgeler kısmından lütfen kontrol ediniz!",
                IsSolved = true,
                IsTeacherRead = false,
                IsCompanyRead = false,
                IsStudentRead = false
            };
        }

        public static Message DeleteRelation(string teacherName,string studentName, string companyName)
        {
            return new Message
            {
                MessageDate = DateTime.Now,
                Subject = $"{studentName} adlı öğrenci {companyName} adlı şirketten çıkartıldı!",
                MessageDetail = $"İşlem {teacherName} tarafından gerçekleştirildi. Eğer staj işlemleriniz bitmediyse lütfen ilgi danışmana <strong>{teacherName}</strong> ulaşıp bilgi alınız!",
                IsSolved = true,
                IsTeacherRead = false,
                IsCompanyRead = false,
                IsStudentRead = false
            };
        }

        public static Message AddRelation(string teacherName, string studentName, string companyName)
        {
            return new Message
            {
                MessageDate = DateTime.Now,
                Subject = $"{studentName} adlı öğrenci {companyName} adlı şirkettine atandı!",
                MessageDetail = $"İşlem {teacherName} tarafından gerçekleştirildi. Lütfen staj dökümanınızı {companyName} şirketindeki ilgili kişiye imzalatınız! Artık dökümanlarınız ve staj defterleriniz {companyName} şirketi tarafından görüntülenebilecektir.",
                IsSolved = true,
                IsTeacherRead = false,
                IsCompanyRead = false,
                IsStudentRead = false
            };
        }
    }
}
