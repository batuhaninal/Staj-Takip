using Core.Entities.Concrete;
using Core.Utilities.Results;
using StajTakip.Business.Abstract;
using StajTakip.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimRepository _userOperationRepo;

        public UserOperationClaimManager(IUserOperationClaimRepository userOperationRepo)
        {
            _userOperationRepo = userOperationRepo;
        }

        public IResult Add(UserOperationClaim userClaim)
        {
            _userOperationRepo.Add(userClaim);
            return new SuccessResult();
        }
    }
}
