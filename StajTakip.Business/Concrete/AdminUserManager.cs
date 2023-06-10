using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using StajTakip.Business.Abstract;
using StajTakip.DataAccess.Abstract;
using StajTakip.DataAccess.Concrete.Contexts;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Concrete
{
    public class AdminUserManager : IAdminUserService
    {
        private readonly IAdminUserRepository _adminRepo;
        private readonly StajTakipContext _context = new StajTakipContext();

        public AdminUserManager(IAdminUserRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public IResult Add(AdminUser adminUser)
        {
            _adminRepo.Add(adminUser);
            return new SuccessResult();
        }

        public IDataResult<AdminUser> GetByAdminUserId(int adminUserId)
        {
            var result = BusinessRules.Run(CheckByAdminUserId(adminUserId));
            if (result == null)
            {
                var data = _adminRepo.Get(x => x.Id == adminUserId);
                return new SuccessDataResult<AdminUser>(data);
            }

            return new ErrorDataResult<AdminUser>(result.Message ?? "Beklenmeyen bir hata meydana geldi!");
        }

        public IDataResult<AdminUser> GetByUserId(int userId)
        {
            var result = BusinessRules.Run(CheckByUserId(userId));
            if(result == null)
            {
                var data = _adminRepo.Get(x=>x.UserId == userId);
                return new SuccessDataResult<AdminUser>(data);
            }

            return new ErrorDataResult<AdminUser>(result.Message);
        }

        private IResult CheckByUserId(int userId)
        {
            var result = _adminRepo.Get(x=>x.UserId == userId);
            if (result == null)
                return new ErrorResult("Kullanici bulunamadi!");

            return new SuccessResult();
        }

        private IResult CheckByAdminUserId(int adminId)
        {
            var result = _adminRepo.Get(x => x.Id == adminId);
            if (result == null)
                return new ErrorResult("Kullanici bulunamadi!");

            return new SuccessResult();
        }

        public IDataResult<List<AdminUser>> GetAllAdminWithRelations()
        {
            var companies = _context.AdminUsers.Where(x=>x.IsCompany == true)
                .Include(x=>x.AdminStudentRelations)
                .ThenInclude(x=>x.StudentUser)
                .ToList();
            return new SuccessDataResult<List<AdminUser>>(companies);
        }
    }
}
