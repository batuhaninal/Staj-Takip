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
    public class StudentUserManager : IStudentUserService
    {
        private readonly IStudentUserRepository _studentRepo;

        public StudentUserManager(IStudentUserRepository studentRepo)
        {
            _studentRepo = studentRepo;
        }

        public IResult Add(StudentUser studentUser)
        {
            _studentRepo.Add(studentUser);
            return new SuccessResult();
        }
    }
}
