﻿using Core.Utilities.Business;
using Core.Utilities.Results;
using StajTakip.Business.Abstract;
using StajTakip.Business.Constants;
using StajTakip.DataAccess.Abstract;
using StajTakip.Entities.ComplexTypes;
using StajTakip.Entities.Concrete;

namespace StajTakip.Business.Concrete
{
    public class InternshipDocumentManager : IInternshipDocumentService
    {
        private readonly IInternshipDocumentRepository _internshipDocumentRepository;
        private readonly IAdminUserService _adminUserService;
        private readonly IStudentUserService _studentUserService;
        private readonly IMessageService _messageService;

        public InternshipDocumentManager(IInternshipDocumentRepository internshipDocumentRepository, IMessageService messageService)
        {
            _internshipDocumentRepository = internshipDocumentRepository;
            _messageService = messageService;
        }

        public IDataResult<InternshipDocument> AcceptDocument(InternshipDocument entity, int adminUserId)
        {
            var result = BusinessRules.Run(IsExist(entity.Id));
            if (result == null)
            {
                var data = _internshipDocumentRepository.Update(entity);

                var admin = _adminUserService.GetByAdminUserId(adminUserId);

                var student = _studentUserService.GetById(entity.StudentUserId);

                var messageResult = _messageService.AcceptDocument(entity.Id, entity.DocumentName,$"{admin.Data.FirstName} {admin.Data.LastName}",student.Data.StudentNumber,adminUserId, student.Data.UserId);
                if (!messageResult.Success)
                    return new SuccessDataResult<InternshipDocument>(data, messageResult.Message ?? "Bildirim gönderilemedi fakat döküman onaylandı!");

                return new SuccessDataResult<InternshipDocument>(data);
            }

            return new ErrorDataResult<InternshipDocument>(result.Message);
        }

        public IResult Add(InternshipDocument entity)
        {
            _internshipDocumentRepository.Add(entity);
            var messageResult = _messageService.SendDocumentAdded(entity.StudentUserId,entity.Id,entity.DocumentName);
            if (!messageResult.Success)
            {
                return new SuccessResult(messageResult.Message ?? "Mesaj gönderilemedi fakat döküman başarılı şekilde eklendi!");
            }

            return new SuccessResult();
        }

        public IDataResult<InternshipDocument> Get(int id)
        {
            var result = BusinessRules.Run(IsExist(id));
            if (result == null)
            {
                var data = _internshipDocumentRepository.Get(x => x.Id == id);
                return new SuccessDataResult<InternshipDocument>(data);
            }
            return new ErrorDataResult<InternshipDocument>(result.Message);
        }

        public IDataResult<List<InternshipDocument>> GetAll()
        {
            var data = _internshipDocumentRepository.GetAll().ToList();
            return new SuccessDataResult<List<InternshipDocument>>(data);
        }

        public IDataResult<List<InternshipDocument>> GetAllByStudentId(int studentId)
        {
            var data = _internshipDocumentRepository.GetAll(x=>x.StudentUserId == studentId).ToList();
            return new SuccessDataResult<List<InternshipDocument>>(data);
        }

        public IResult HardDelete(InternshipDocument entity)
        {
            var result = BusinessRules.Run(IsExist(entity.Id));
            if (result == null)
            {
                _internshipDocumentRepository.Delete(entity);
                return new SuccessResult();
            }
            return result;
        }

        public IResult HardDelete(int id)
        {
            var result = BusinessRules.Run(IsExist(id));
            if (result == null)
            {
                var data = _internshipDocumentRepository.Get(x => x.Id == id);
                _internshipDocumentRepository.Delete(data);
                return new SuccessResult();
            }
            return result;
        }

        public IDataResult<InternshipDocument> RejectDocument(InternshipDocument entity, int adminUserId)
        {
            var result = BusinessRules.Run(IsExist(entity.Id));
            if (result == null)
            {
                var data = _internshipDocumentRepository.Update(entity);

                var messageResult = _messageService.RejectDocument(data.StudentUserId, adminUserId, data.Id, data.DocumentName);
                if (!messageResult.Success)
                    return new SuccessDataResult<InternshipDocument>(data, messageResult.Message ?? "Bildirim gönderilemedi fakat döküman onaylandı!");

                return new SuccessDataResult<InternshipDocument>(data);
            }

            return new ErrorDataResult<InternshipDocument>(result.Message);
        }

        public IDataResult<InternshipDocument> Update(InternshipDocument entity)
        {
            var result = BusinessRules.Run(IsExist(entity.Id));
            if (result == null)
            {
                var data = _internshipDocumentRepository.Update(entity);
                return new SuccessDataResult<InternshipDocument>(data);
            }

            return new ErrorDataResult<InternshipDocument>(result.Message);
        }

        public IDataResult<InternshipDocument> SignDocument(InternshipDocument entity, int userId, Roles role)
        {
            var result = BusinessRules.Run(IsExist(entity.Id));
            if (result == null)
            {
                var data = _internshipDocumentRepository.Update(entity);
                var messageResult = _messageService.SendSignedDocumentNoty(userId, data.Id, data.StudentUserId, data.DocumentName, role);
                if (!messageResult.Success)
                    return new SuccessDataResult<InternshipDocument>(data, "Belge başarıyla imzalandı fakat"+ result.Message ??  "Bildirim mesajı gönderilemedi!");
                return new SuccessDataResult<InternshipDocument>(data);
            }

            return new ErrorDataResult<InternshipDocument>(result.Message);
        }
        private IResult IsExist(int id)
        {
            var result = _internshipDocumentRepository.Any(x => x.Id == id);
            if (result)
            {
                return new SuccessResult();
            }

            return new ErrorResult(Messages.IsExist);
        }
    }
}
