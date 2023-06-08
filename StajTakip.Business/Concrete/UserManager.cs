using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using StajTakip.Business.Abstract;
using StajTakip.DataAccess.Abstract;
using StajTakip.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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

        public IResult ChangePassword(ChangePasswordDto model)
        {
            var result = BusinessRules.Run(CheckUsernameByUserId(model.UserId,model.Username), CheckUserPassword(model.UserId, model.OldPassword));
            if(result != null)
                return result;

            var user = _userRepository.Get(x=>x.Username ==  model.Username);
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(model.NewPassword, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            var updatedUser = Update(user);
            if (!updatedUser.Success)
                return new ErrorResult(updatedUser.Message);

            return new SuccessResult();
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

        public IDataResult<User> Update(User user)
        {
            var result = BusinessRules.Run(CheckUser(user.Id));
            if (result != null)
                return new ErrorDataResult<User>(result.Message);
            var oldUser = _userRepository.Get(x=>x.Id == user.Id);

            user.CreatedDate = oldUser.CreatedDate;
            _userRepository.Update(user);
            return new SuccessDataResult<User>(user);
        }

        private IResult CheckUsernameByUserId(int userId, string userName)
        {
            var result = _userRepository.Get(x => x.Username == userName && x.Id == userId);
            if (result is null)
                return new ErrorResult("Kullanıcı adınız yanlış!");

            return new SuccessResult();
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

        private IResult CheckUserPassword(int userId, string password)
        {
            var data = _userRepository.Get(x=>x.Id == userId);

            if (data == null)
                return new ErrorResult("Kullanıcı bulunamadı!");

            var result = HashingHelper.VerifyPasswordHash(password, data.PasswordHash, data.PasswordSalt);
            if (!result)
                return new ErrorResult("Lütfen eski şifrenizi kontrol ediniz!");

            return new SuccessResult();
        }
    }
}
