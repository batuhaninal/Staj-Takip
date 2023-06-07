using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.Concrete
{
    public class AdminStudentRelation : IEntity
    {
        public int Id { get; set; }
        public int? AdminUserId { get; set; }
        public virtual AdminUser? AdminUser { get; set; }
        public int? StudentUserId { get; set; }
        public virtual StudentUser? StudentUser { get; set; }
        public bool IsCompany { get; set; }
    }
}
