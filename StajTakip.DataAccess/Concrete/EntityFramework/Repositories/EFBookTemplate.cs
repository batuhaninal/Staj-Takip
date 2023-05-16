using Core.DataAccess.Concrete.EntityFramework;
using StajTakip.DataAccess.Abstract;
using StajTakip.DataAccess.Concrete.Contexts;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.DataAccess.Concrete.EntityFramework.Repositories
{
    public class EFBookTemplate : EFEntityRepository<BookTemplate, StajTakipContext>, IBookTemplateRepository
    {
    }
}
