using Core.Utilities.Results;
using StajTakip.Business.Abstract;
using StajTakip.DataAccess.Abstract;
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

        public AdminUserManager(IAdminUserRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public IResult Add(AdminUser adminUser)
        {
            _adminRepo.Add(adminUser);
            return new SuccessResult();
        }
    }
}
