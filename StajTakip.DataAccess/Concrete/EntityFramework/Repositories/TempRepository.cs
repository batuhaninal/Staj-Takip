using Core.DataAccess.Concrete.EntityFramework;
using StajTakip.DataAccess.Abstract;
using StajTakip.DataAccess.Concrete.Contexts;
using StajTakip.Entities.Concrete;

namespace StajTakip.DataAccess.Concrete.EntityFramework.Repositories
{
    public class TempRepository : EFEntityRepository<Temp, StajTakipContext>, ITempRepository
    {
    }
}
