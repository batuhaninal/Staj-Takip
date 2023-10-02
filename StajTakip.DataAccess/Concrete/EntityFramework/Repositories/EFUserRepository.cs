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
    public class EFUserRepository : EFEntityRepository<User, StajTakipContext>, IUserRepository
    {
        public List<OperationClaim> GetClaims(int userId)
        {
            using (var context = new StajTakipContext())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                             on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == userId
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
                return result.ToList();
            }
        }
    }
}
