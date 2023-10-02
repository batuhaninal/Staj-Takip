using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.Concrete
{
    public class CreatedMessageMessage
    {
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
    }
}
