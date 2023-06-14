using Core.Utilities.Results;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Abstract
{
    public interface IStudentUserService
    {
        IResult Add(StudentUser studentUser);
        IDataResult<StudentUser> GetByUserId(int userId);
        IDataResult<StudentUser> GetByIdWithRelations(int studentId);
        IDataResult<List<StudentUser>> GetAll();
        IDataResult<StudentUser> GetById(int studentUserId);
    }
}
