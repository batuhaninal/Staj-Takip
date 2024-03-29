﻿using Core.Entities.Abstract;
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
    public class AdminStudentRelationManager : IAdminStudentRelationService
    {
        private readonly IAdminStudentRelationRepository _repo;

        public AdminStudentRelationManager(IAdminStudentRelationRepository repo)
        {
            _repo = repo;
        }

        public IResult Add(AdminStudentRelation entity)
        {
            var result = BusinessRules.Run(CheckEntity(entity), CheckEntityDuplicate(entity), CheckStudentHasCompanyRelation(entity.StudentUserId.Value));
            if (result != null)
            {
                return new ErrorResult(result.Message);
            }
            _repo.Add(entity);
            return new SuccessResult();
        }

        public IResult AddForCompany(AdminStudentRelation entity)
        {
            entity.IsCompany = true;
            var result = BusinessRules.Run(CheckEntity(entity), CheckEntityDuplicate(entity), CheckStudentHasCompanyRelation(entity.StudentUserId.Value));
            if (result != null)
            {
                return new ErrorResult(result.Message);
            }
            _repo.Add(entity);
            return new SuccessResult();
        }

        public IResult AddForTeacher(AdminStudentRelation entity)
        {
            entity.IsCompany = false;
            var result = BusinessRules.Run(CheckEntity(entity), CheckEntityDuplicate(entity), CheckStudenHasTeacherRelation(entity.StudentUserId.Value));
            if (result != null)
            {
                return new ErrorResult(result.Message);
            }
            _repo.Add(entity);
            return new SuccessResult();
        }    

        public IDataResult<AdminStudentRelation> Get(int id)
        {
            var result = BusinessRules.Run(CheckEntityId(id));
            if (result != null)
            {
                return new ErrorDataResult<AdminStudentRelation>(result.Message);
            }
            var model = _repo.Get(x => x.Id == id, x => x.StudentUser, x => x.AdminUser);
            return new SuccessDataResult<AdminStudentRelation>(model);
        }

        public IDataResult<List<AdminStudentRelation>> GetAllByAdminIdWithStudent(int adminId)
        {
            var result = _repo.GetAll(x => x.AdminUserId == adminId, x => x.StudentUser).ToList();
            return new SuccessDataResult<List<AdminStudentRelation>>(result);
        }

        public IDataResult<List<AdminStudentRelation>> GetAllByCompanyIdWithStudent(int companyId)
        {
            var result = _repo.GetAll(x => x.AdminUserId == companyId && x.IsCompany == true, x => x.StudentUser).ToList();
            return new SuccessDataResult<List<AdminStudentRelation>>(result);
        }

        public IDataResult<List<AdminStudentRelation>> GetAllByStudentIdWithAdmin(int studentId)
        {
            var result = _repo.GetAll(x => x.StudentUserId == studentId, x=>x.StudentUser, x => x.AdminUser).ToList();
            return new SuccessDataResult<List<AdminStudentRelation>>(result);
        }

        public IDataResult<List<AdminStudentRelation>> GetAllByStudentIdWithCompany(int studentId)
        {
            var result = _repo.GetAll(x => x.StudentUserId == studentId && x.AdminUser.IsCompany == true, x => x.AdminUser).ToList();
            return new SuccessDataResult<List<AdminStudentRelation>>(result);
        }

        public IDataResult<List<AdminStudentRelation>> GetAllWithUsers()
        {
            var result = _repo.GetAll(null, x => x.AdminUser, x => x.StudentUser).ToList();
            return new SuccessDataResult<List<AdminStudentRelation>>(result);
        }

        public IResult HardDelete(int id)
        {
            var result = BusinessRules.Run(CheckEntityId(id));
            if (result != null)
            {
                return new ErrorResult(result.Message);
            }
            var model = _repo.Get(x => x.Id == id);
            _repo.Delete(model);
            return new SuccessResult();
        }

        public IResult HardDelete(AdminStudentRelation entity)
        {
            var result = BusinessRules.Run(CheckEntity(entity), CheckEntityId(entity.Id));
            if (result != null)
            {
                return new ErrorResult(result.Message);
            }
            _repo.Delete(entity);
            return new SuccessResult();
        }

        public IDataResult<AdminStudentRelation> Update(AdminStudentRelation entity)
        {
            var result = BusinessRules.Run(CheckEntity(entity), CheckEntityId(entity.Id));
            if (result != null)
            {
                return new ErrorDataResult<AdminStudentRelation>(result.Message);
            }
            var model = _repo.Update(entity);
            return new SuccessDataResult<AdminStudentRelation>(model);
        }

        private IResult CheckEntity(AdminStudentRelation entity)
        {
            if (entity is null)
                return new ErrorResult("Lütfen alanları doldurunuz!");

            return new SuccessResult();
        }

        private IResult CheckEntityDuplicate(AdminStudentRelation entity)
        {
            var result = _repo.Get(x => x.StudentUserId == entity.StudentUserId && x.AdminUserId == entity.AdminUserId);
            if (result != null)
                return new ErrorResult("Öğrenci zaten mevcut eğitmene atanmıştır!");
            return new SuccessResult();
        }

        private IResult CheckEntityId(int id)
        {
            var entity = _repo.Get(x => x.Id == id);
            if (entity is null)
                return new ErrorResult("Verilen parametrede bir veri bulunamadı!");

            return new SuccessResult();
        }

        private IResult CheckStudenHasTeacherRelation(int studentUserId)
        {
            var result = _repo.Get(x => x.StudentUserId == studentUserId && x.IsCompany == false, x=>x.AdminUser);
            if (result != null)
                return new ErrorResult($"Kullanıcı başka bir eğitmene atanmıştır. Lütfen {result.AdminUser.FirstName} {result.AdminUser.LastName} eğitmeni ile iletişime geçiniz!");

            return new SuccessResult();
        }

        private IResult CheckStudentHasCompanyRelation(int studentUserId)
        {
            var result = _repo.Get(x => x.StudentUserId == studentUserId && x.IsCompany == true, x=>x.AdminUser);
            if (result != null)
                return new ErrorResult($"Öğrenci {result.AdminUserId} id değerine {result.AdminUser.FirstName} {result.AdminUser.LastName} sahip başka bir şirkete kayıtlı! Lütfen şirketinden çıkartıp yeniden deneyiniz!");


            return new SuccessResult();
        }
    }
}
