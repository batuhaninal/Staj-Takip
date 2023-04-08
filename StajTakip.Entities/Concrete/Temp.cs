using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.Concrete
{
    public class Temp : EntityBase
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
