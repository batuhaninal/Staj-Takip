﻿using Core.Entities.Abstract;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.Concrete
{
    public class StudentUser : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        //public int AdminUserId { get; set; }
        //public virtual AdminUser? AdminUser { get; set; }

        public virtual ICollection<InternshipsBook>? InternshipsBooks { get; set; }
    }
}
