using Core.Utilities.Business;
using Core.Utilities.Results;
using StajTakip.Business.Abstract;
using StajTakip.DataAccess.Abstract;
using StajTakip.Entities.Concrete;

namespace StajTakip.Business.Concrete
{
    public class BookTemplateManager : IBookTemplateService
    {
        private readonly IBookTemplateRepository _bookTemplateRepo;

        public BookTemplateManager(IBookTemplateRepository bookTemplateRepo)
        {
            _bookTemplateRepo = bookTemplateRepo;
        }

        public IResult Add(BookTemplate entity)
        {
            var result = BusinessRules.Run(CheckEntity(entity), CheckIsCurrent(entity));
            if (result != null)
                return result;

            _bookTemplateRepo.Add(entity);
            return new SuccessResult();
        }

        public IDataResult<BookTemplate> Get(int id)
        {
            var result = BusinessRules.Run(CheckById(id));
            if (result != null)
                return new ErrorDataResult<BookTemplate>(result.Message);

            var bookTemplate = _bookTemplateRepo.Get(x => x.Id == id);
            return new SuccessDataResult<BookTemplate>(bookTemplate);
        }

        public IDataResult<BookTemplate> GetCurrent()
        {
            var bookTemplate = _bookTemplateRepo.Get(x => x.IsCurrent == true);
            return new SuccessDataResult<BookTemplate>(bookTemplate);
        }

        public IDataResult<List<BookTemplate>> GetAll()
        {
            var bookTemplates = _bookTemplateRepo.GetAll().ToList();
            if (bookTemplates.Count < 1)
                return new ErrorDataResult<List<BookTemplate>>("Henuz veri girilmemis!");

            return new SuccessDataResult<List<BookTemplate>>(bookTemplates);
        }

        public IResult HardDelete(int id)
        {
            var result = BusinessRules.Run(CheckById(id));
            if (result != null)
                return result;

            var template = _bookTemplateRepo.Get(x => x.Id == id);
            _bookTemplateRepo.Delete(template);
            return new SuccessResult();
        }

        public IDataResult<BookTemplate> Update(BookTemplate entity)
        {
            var result = BusinessRules.Run(CheckById(entity.Id), CheckIsCurrent(entity));
            if (result != null)
                return new ErrorDataResult<BookTemplate>(result.Message);

            _bookTemplateRepo.Update(entity);
            return new SuccessDataResult<BookTemplate>(entity);
        }

        private IResult CheckEntity(BookTemplate entity)
        {
            if (entity is null)
                return new ErrorResult("Lütfen alanları kontrol ediniz!");

            return new SuccessResult();
        }

        private IResult CheckById(int id)
        {
            var result = _bookTemplateRepo.Get(x => x.Id == id);
            if (result is null)
                return new ErrorResult("Bu paramatreye ait bir veri bulunamadi!");

            return new SuccessResult();
        }

        private IResult CheckIsCurrent(BookTemplate entity)
        {
            if (entity.IsCurrent)
            {
                var lastTemplate = _bookTemplateRepo.Get(x => x.IsCurrent == true);
                if (lastTemplate != null)
                {
                    lastTemplate.IsCurrent = false;
                    var result = _bookTemplateRepo.Update(lastTemplate);
                    if (result == null)
                        return new ErrorResult("Geçerli template işlemi yapılamadı!");
                }

            }
            return new SuccessResult();
        }
    }
}
