using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.Concrete
{
    public class CreatedMailMessage
    {
        public string[] ReceiverMail { get; set; }
        public string SenderMail { get; set; }

        // Staj takip formu 1. asama
        public string Subject { get; set; }

        public string MessageBody { get; set; }
    }
}
