using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.DTOs
{
    public class EmailSendDto
    {
        // 191118016@samsun.edu.tr
        public string SenderMail { get; set; }
        public string[] ReceiverMail { get; set; }

        // Staj takip formu 1. asama
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
