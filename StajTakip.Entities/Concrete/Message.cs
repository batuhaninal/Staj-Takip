﻿using Core.Entities.Abstract;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.Concrete
{
    public class Message : EntityBase
    {
        public int? SenderId { get; set; }
        public virtual User? SenderUser { get; set; }
        public int? ReceiverId { get; set; }
        public virtual User? ReceiverUser { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public DateTime MessageDate { get; set; }
    }
}