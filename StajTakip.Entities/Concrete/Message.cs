﻿using Core.Entities.Abstract;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.Concrete
{
    public class Message : IEntity
    {
        public int Id { get; set; }
        public int? SenderUserId { get; set; }
        public virtual User? SenderUser { get; set; }
        public int? ReceiverUserId { get; set; }
        public virtual User? ReceiverUser { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public DateTime MessageDate { get; set; } = DateTime.Now;
        public bool IsSolved { get; set; } = false;
        public bool IsTeacherRead { get; set; } = false;
        public bool IsCompanyRead { get; set; } = false;
        public bool IsStudentRead { get; set; } = false;
    }
}
