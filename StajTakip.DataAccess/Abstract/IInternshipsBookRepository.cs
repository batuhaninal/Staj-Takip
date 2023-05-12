using Core.DataAccess.Abstract;
using Core.DataAccess.Concrete.EntityFramework;
using Core.Utilities.Results;
using StajTakip.DataAccess.Concrete.Contexts;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.DataAccess.Abstract
{
    public interface IInternshipsBookRepository : IEntityRepository<InternshipsBook> 
    {
    }
}
