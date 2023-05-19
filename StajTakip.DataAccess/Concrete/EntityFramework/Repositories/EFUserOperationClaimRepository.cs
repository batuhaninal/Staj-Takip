using Core.DataAccess.Concrete.EntityFramework;
using Core.Entities.Concrete;
using StajTakip.DataAccess.Abstract;
using StajTakip.DataAccess.Concrete.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.DataAccess.Concrete.EntityFramework.Repositories
{
    public class EFUserOperationClaimRepository : EFEntityRepository<UserOperationClaim, StajTakipContext>, IUserOperationClaimRepository
    {
    }
}
