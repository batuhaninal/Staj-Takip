using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.Concrete
{
    public class BookTemplate : EntityBase
    {
        public string Template { get; set; }
        public string Title { get; set; }
    }
}
