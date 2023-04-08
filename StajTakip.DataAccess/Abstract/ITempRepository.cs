using Core.DataAccess.Abstract;
using Core.DataAccess.Concrete.EntityFramework;
using StajTakip.DataAccess.Concrete.Contexts;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.DataAccess.Abstract
{
    public interface ITempRepository : IEntityRepository<Temp>
    {
    }
}
