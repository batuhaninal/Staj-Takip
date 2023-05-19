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
    public class UserManager : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IDataResult<User> Add(User user)
        {
            var result = _userRepository.Add(user);
            if(result != null)
                return new SuccessDataResult<User>(result);

            return new ErrorDataResult<User>("Kullanici eklenemedi!");
        }

        public IDataResult<User> GetByMail(string email)
        {
            var result = _userRepository.Get(x=>x.Email == email);
            if (result is null)
                return new ErrorDataResult<User>("Bu mailde bir kullanici bulunamadi!");
            return new SuccessDataResult<User>(result);
        }

        public IDataResult<List<OperationClaim>> GetClaims(int userId)
        {
            var claims = _userRepository.GetClaims(userId);
            if(claims.Count > 0)
                return new SuccessDataResult<List<OperationClaim>>(claims);
            return new ErrorDataResult<List<OperationClaim>>("Kullanici bulunamadi!");
        }
    }
}
