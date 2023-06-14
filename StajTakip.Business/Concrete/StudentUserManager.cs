using Core.Utilities.Business;
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

        public IDataResult<List<StudentUser>> GetAll()
        {
            var data = _studentRepo.GetAll().ToList();
            if(data.Count > 0)
            {
                return new SuccessDataResult<List<StudentUser>>(data);
            }
            return new ErrorDataResult<List<StudentUser>>("Henüz kullanıcı yok!");
        }

        public IDataResult<StudentUser> GetById(int studentUserId)
        {
            var result = BusinessRules.Run(CheckByStudentUserId(studentUserId));
            if (result != null)
                return new ErrorDataResult<StudentUser>(result.Message ?? "Beklenmeyen bir hatayla karşılaşıldı!");

            var user = _studentRepo.Get(x=>x.Id == studentUserId);
            return new SuccessDataResult<StudentUser>(user);
        }

        public IDataResult<StudentUser> GetByUserId(int userId)
        {
            var result = BusinessRules.Run(CheckByUserId(userId));
            if (result == null)
            {
                var data = _studentRepo.Get(x => x.UserId == userId);
                return new SuccessDataResult<StudentUser>(data);
            }

            return new ErrorDataResult<StudentUser>(result.Message);
        }

        private IResult CheckByUserId(int userId)
        {
            var result = _studentRepo.Get(x => x.UserId == userId);
            if (result == null)
                return new ErrorResult("Kullanici bulunamadi!");

            return new SuccessResult();
        }

        private IResult CheckByStudentUserId(int studentId)
        {
            var result = _studentRepo.Get(x=>x.Id == studentId);
            if (result == null)
                return new ErrorResult("Kullanici bulunamadi!");

            return new SuccessResult();
        }

        public IDataResult<StudentUser> GetByIdWithRelations(int studentId)
        {
            var result = BusinessRules.Run(CheckByStudentUserId(studentId));
            if(result != null)
                return new ErrorDataResult<StudentUser>(result.Message);

            var user = _studentRepo.Get(x=>x.Id ==studentId, x=>x.AdminStudentRelations);

            return new SuccessDataResult<StudentUser>(user);
        }
    }
}
