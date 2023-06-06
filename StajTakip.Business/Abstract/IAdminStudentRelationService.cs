using Core.Utilities.Results;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Abstract
{
    public interface IAdminStudentRelationService
    {
        IResult Add(AdminStudentRelation entity);
        IResult HardDelete(AdminStudentRelation entity);
        IResult HardDelete(int id);
        IDataResult<AdminStudentRelation> Update(AdminStudentRelation entity);
        IDataResult<AdminStudentRelation> Get(int id);
        IDataResult<List<AdminStudentRelation>> GetAllWithUsers();
        IDataResult<List<AdminStudentRelation>> GetAllByStudentIdWithAdmin(int studentId);
        IDataResult<List<AdminStudentRelation>> GetAllByAdminIdWithStudent(int adminId);
    }
}
