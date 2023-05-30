using Core.Entities.Concrete;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Abstract
{
    public interface IUserService
    {
        IDataResult<List<OperationClaim>> GetClaims(int userId);
        IDataResult<User> Add(User user);
        IDataResult<User> GetByUsername(string username);
        IDataResult<User> GetById(int userId);
    }
}
