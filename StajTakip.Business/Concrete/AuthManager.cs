using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using StajTakip.Business.Abstract;
using StajTakip.Business.Constants;
using StajTakip.DataAccess.Abstract;
using StajTakip.Entities.Concrete;
using StajTakip.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IAdminUserService _adminUserService;
        private readonly IStudentUserService _studentUserService;
        private readonly IAdminStudentRelationService _adminStudentRelationService;

        public AuthManager(IUserService userService, IUserOperationClaimService userOperationClaimService, IAdminUserService adminUserService, IStudentUserService studentUserService, IAdminStudentRelationService adminStudentRelationService)
        {
            _userService = userService;
            _userOperationClaimService = userOperationClaimService;
            _adminUserService = adminUserService;
            _studentUserService = studentUserService;
            _adminStudentRelationService = adminStudentRelationService;
        }

        public IDataResult<List<OperationClaim>> GetClaims(int userId)
        {
            var result = _userService.GetClaims(userId);
            if(result.Success)
                return new SuccessDataResult<List<OperationClaim>>(result.Data);

            return new ErrorDataResult<List<OperationClaim>>("Kullanici bulunamadi!");
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByUsername(userForLoginDto.Username).Data;
            if (userToCheck == null)
                return new ErrorDataResult<User>("Kullanici bulunamadi!");

            var result = HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt);
            if (!result)
                return new ErrorDataResult<User>("Kullanici adi veya parola yanlis!");

            return new SuccessDataResult<User>(userToCheck);

        }

        public IDataResult<User> RegisterAdmin(AdminUserForRegisterDto userForRegisterDto)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Username = userForRegisterDto.Username,
                Email = userForRegisterDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            var addedUser = _userService.Add(user);
            if(!addedUser.Success)
                return new ErrorDataResult<User>(addedUser.Message ?? "Hata Olustu!");

            var adminInfo = new AdminUser
            {
                UserId = addedUser.Data.Id,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                IsCompany = userForRegisterDto.IsCompany,
            };

            var res = _adminUserService.Add(adminInfo);

            var operationClaim = new UserOperationClaim
            {
                // Egitmen veya sirket
                OperationClaimId = userForRegisterDto.IsCompany ? 3 : 2,
                UserId = addedUser.Data.Id
            };
            var result = _userOperationClaimService.Add(operationClaim);
            if (!result.Success)
                return new ErrorDataResult<User>("Hata!");

            if (userForRegisterDto.IsCompany)
            {
                if (userForRegisterDto.StudentId != 0 && userForRegisterDto.StudentId != null)
                {
                    var relation = new AdminStudentRelation
                    {
                        AdminUserId = adminInfo.Id,
                        IsCompany = true,
                        StudentUserId = userForRegisterDto.StudentId
                    };
                    var relationResult = _adminStudentRelationService.AddForCompany(relation);
                    if (!relationResult.Success)
                        return new ErrorDataResult<User>(relationResult.Message ?? "Öğrenci şirkete atanamadı!");
                }
            }

            return new SuccessDataResult<User>(user);
        }

        public IDataResult<User> RegisterStudent(StudentUserForRegisterDto userForRegisterDto)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Username = userForRegisterDto.Username,
                Email = userForRegisterDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            var addedUser = _userService.Add(user);
            if (!addedUser.Success)
                return new ErrorDataResult<User>(addedUser.Message ?? "Hata Olustu!");

            var studentInfo = new StudentUser
            {
                UserId = addedUser.Data.Id,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
            };
            
            _studentUserService.Add(studentInfo);

            var operationClaim = new UserOperationClaim
            {
                // Student
                OperationClaimId = 4,
                UserId = addedUser.Data.Id
            };
            var result = _userOperationClaimService.Add(operationClaim);
            if (!result.Success)
                return new ErrorDataResult<User>("Hata!");

            return new SuccessDataResult<User>(user);
        }

        public IResult UserExists(string username)
        {
            if (_userService.GetByUsername(username) != null)
            {
                return new ErrorResult("Bu mail ile kayitli bir kullanici bulunmaktadir!");
            }
            return new SuccessResult();
        }
    }
}
