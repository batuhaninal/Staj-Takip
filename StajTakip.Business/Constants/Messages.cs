using Core.Entities.Concrete;
using StajTakip.Entities.Concrete;
using StajTakip.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
