using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.DTOs
{
    public class EmailSendDto
    {
        public string SenderMail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
