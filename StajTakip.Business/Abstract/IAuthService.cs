using Core.Entities.Concrete;
using Core.Utilities.Results;
using StajTakip.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> RegisterAdmin(AdminUserForRegisterDto userForRegisterDto);
        IDataResult<User> RegisterStudent(StudentUserForRegisterDto userForRegisterDto);
        IDataResult<User> Login(UserForLoginDto userForLoginDto);
        IResult UserExists(string email);
        IDataResult<List<OperationClaim>> GetClaims(int userId);
    }
}
