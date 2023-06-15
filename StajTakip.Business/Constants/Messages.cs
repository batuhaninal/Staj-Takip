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
                IsSolved = false
            };
        }

        public static Message StudentAddedDocument(string userFirstLastName, int documentId, string documentName)
        {
            return new Message
            {
                MessageDate = DateTime.Now,
                Subject = $"{userFirstLastName} öğrencisi {documentId} id değerine sahip {documentName} isimli yeni döküman ekledi!",
                MessageDetail = $"Lütfen eklenen {documentId} id değerine sahip dökümanı inceleyip gerekli işlemleri yapınız!",
                IsSolved = false
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
            };
        }
    }
}
