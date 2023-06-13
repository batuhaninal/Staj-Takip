using Core.Entities.Abstract;
using Core.Utilities.Business;
using Core.Utilities.Results;
using StajTakip.Business.Abstract;
using StajTakip.Business.Constants;
using StajTakip.DataAccess.Abstract;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Concrete
{
    public class BookImageManager : IBookImageService
    {
        private readonly IBookImageRepository _bookImageRepository;

        public BookImageManager(IBookImageRepository bookImageRepository)
        {
            _bookImageRepository = bookImageRepository;
        }

        public IResult Add(BookImage entity)
        {
            _bookImageRepository.Add(entity);
            return new SuccessResult();
        }

        public IDataResult<BookImage> Get(int id)
        {
            var result = BusinessRules.Run(IsExist(id));
            if (result == null)
            {
                var data = _bookImageRepository.Get(x => x.Id == id);
                return new SuccessDataResult<BookImage>(data);
            }
            return new ErrorDataResult<BookImage>(result.Message);
        }

        public IDataResult<List<BookImage>> GetAll()
        {
            var data = _bookImageRepository.GetAll().ToList();
            return new SuccessDataResult<List<BookImage>>(data);
        }

        public IDataResult<List<BookImage>> GetAllByBookId(int bookId)
        {
            var result = BusinessRules.Run(CheckImageIsExistOnBook(bookId));
            if(result != null)
                return new ErrorDataResult<List<BookImage>>(result.Message ?? "Beklenmeyen bir hata ile karşılaşıldı!");
            var data = _bookImageRepository.GetAll(x => x.InternshipsBookId == bookId).ToList();
            return new SuccessDataResult<List<BookImage>>(data);
        }

        public IResult HardDelete(BookImage entity)
        {
            var result = BusinessRules.Run(IsExist(entity.Id));
            if (result == null)
            {
                _bookImageRepository.Delete(entity);
                return new SuccessResult();
            }

            return result;
        }

        public IResult HardDelete(int id)
        {
            var result = BusinessRules.Run(IsExist(id));
            if (result == null)
            {
                var data = _bookImageRepository.Get(x=>x.Id == id);
                _bookImageRepository.Delete(data);
                return new SuccessResult();
            }

            return result;
        }

        public IDataResult<BookImage> Update(BookImage entity)
        {
            var result = BusinessRules.Run(IsExist(entity.Id));
            if (result == null)
            {
                var data = _bookImageRepository.Update(entity);
                return new SuccessDataResult<BookImage>(data);
            }

            return new ErrorDataResult<BookImage>(result.Message);
        }

        private IResult IsExist(int id)
        {
            var result = _bookImageRepository.Any(x=>x.Id == id);
            if (result)
            {
                return new SuccessResult();
            }

            return new ErrorResult(Messages.IsExist);
        }

        private IResult CheckImageIsExistOnBook(int bookId)
        {
            var result = _bookImageRepository.Any(x=>x.InternshipsBookId == bookId);
            if (!result)
                return new ErrorResult("Henüz resim eklenmemiş!");

            return new SuccessResult();
        }
    }
}
