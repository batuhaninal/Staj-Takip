using Core.Entities.Concrete;
using Core.Utilities.Business;
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
            var result = BusinessRules.Run(CheckUserIsExist(user.Username));
            if (result != null)
                return new ErrorDataResult<User>(result.Message);

            var addedUser = _userRepository.Add(user);
            if(addedUser != null)
                return new SuccessDataResult<User>(addedUser);

            return new ErrorDataResult<User>("Kullanici eklenemedi!");
        }

        public IDataResult<User> GetById(int userId)
        {
            var result = BusinessRules.Run(CheckUser(userId));
            if (result != null)
                return new ErrorDataResult<User>(result.Message);
            var user = _userRepository.Get(x=>x.Id == userId);
            return new SuccessDataResult<User>(user);
        }

        public IDataResult<User> GetByUsername(string username)
        {
            var result = _userRepository.Get(x=>x.Username == username);
            if (result is null)
                return new ErrorDataResult<User>("Bu kullanıcı adına sahip bir kullanici bulunamadi!");
            return new SuccessDataResult<User>(result);
        }

        public IDataResult<List<OperationClaim>> GetClaims(int userId)
        {
            var claims = _userRepository.GetClaims(userId);
            if(claims.Count > 0)
                return new SuccessDataResult<List<OperationClaim>>(claims);
            return new ErrorDataResult<List<OperationClaim>>("Kullanici bulunamadi!");
        }

        private IResult CheckUser(int userId)
        {
            var user = _userRepository.Get(x=>x.Id ==  userId);
            if (user is null)
                return new ErrorResult("Kullanıcı bulunamadı!");
            return new SuccessResult();
        }

        private IResult CheckUserIsExist(string userName)
        {
            var result = _userRepository.Get(x => x.Username == userName);
            if (result is null)
                return new SuccessResult();
            return new ErrorResult("Bu kullanıcı adına sahip bir kullanıcı zaten bulunmaktadır!");
        }
    }
}
