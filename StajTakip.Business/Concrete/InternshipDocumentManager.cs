using Core.Utilities.Business;
using Core.Utilities.Results;
using StajTakip.Business.Abstract;
using StajTakip.Business.Constants;
using StajTakip.DataAccess.Abstract;
using StajTakip.Entities.Concrete;

namespace StajTakip.Business.Concrete
{
    public class InternshipDocumentManager : IInternshipDocumentService
    {
        private readonly IInternshipDocumentRepository _internshipDocumentRepository;
        private readonly IMessageService _messageService;

        public InternshipDocumentManager(IInternshipDocumentRepository internshipDocumentRepository, IMessageService messageService)
        {
            _internshipDocumentRepository = internshipDocumentRepository;
            _messageService = messageService;
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
