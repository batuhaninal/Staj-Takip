using Core.Utilities.Results;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Abstract
{
    public interface IAdminUserService
    {
        IResult Add(AdminUser adminUser);
        IDataResult<AdminUser> GetByUserId(int userId);
        IDataResult<AdminUser> GetByAdminUserId(int adminUserId);
        IDataResult<List<AdminUser>> GetAllAdminWithRelations();
        IDataResult<AdminUser> GetByAdminUserIdWithUser(int adminUserId);
    }
}
