﻿using AutoMapper;
using Core.Utilities.Business;
using Core.Utilities.Results;
using StajTakip.Business.Abstract;
using StajTakip.Business.Constants;
using StajTakip.DataAccess.Abstract;
using StajTakip.Entities.Concrete;
using StajTakip.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StajTakip.Business.Concrete
{
    public class InternshipsBookManager : IInternshipsBookService
    {
        private readonly IInternshipsBookRepository _repository;
        private IMapper _mapper;

        public InternshipsBookManager(IInternshipsBookRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IResult Add(InternshipsBookPageAddDto entity)
        {
            var mappedModel = _mapper.Map<InternshipsBook>(entity);
            var result = BusinessRules.Run(CheckBookCountByStudentId(mappedModel.StudentUserId));
            if (result != null)
                return new ErrorResult(result.Message ?? "Beklenmeyen hata!");
            _repository.Add(mappedModel);
            return new SuccessResult("Ekleme işlemi başarılı!");
        }

        public IResult CheckBook(CheckBookDto entity)
        {
            var result = BusinessRules.Run(IsBookExist(entity.Id));
            if(result == null)
            {
                var data = _repository.Get(x => x.Id == entity.Id);
                data.IsTeacherChecked = entity.IsTeacherChecked;
                data.IsCompanyChecked = entity.IsCompanyChecked;
                var book = _repository.Update(data);
                if(book == null)
                    return new ErrorResult("Bir hata olustu");

                return new SuccessResult();
            }
            return new ErrorResult(result.Message);
        }

        public IDataResult<InternshipsBook> Get(int id)
        {
            var result = BusinessRules.Run(IsBookExist(id));
            if(result == null)
            {
                var data = _repository.Get(x => x.Id == id);
                return new SuccessDataResult<InternshipsBook>(data);
            }

            return new ErrorDataResult<InternshipsBook>(result.Message);            
        }

        public IDataResult<List<InternshipsBook>> GetAll()
        
        {
            var data = _repository.GetAll().ToList();
            if(data.Count < 1)
            {
                return new ErrorDataResult<List<InternshipsBook>>("Henuz veri girilmemis!");
            }
            return new SuccessDataResult<List<InternshipsBook>>(data);
        }

        // Degistirilmeli
        public IDataResult<List<InternshipsBook>> GetAllPagesWithImagesAndSignatures(int userId)
        {
            // InternshipsBook tablosuna alan eklenmeli
            //List<Expression<Func<InternshipsBook, object>>> includes = new();
            //includes.Add(x => x.Signatures);
            //includes.Add(x => x.BookImages);
            throw new NotImplementedException();
        }

        public int GetCount(int userId)
        {
            return _repository.Count(x=>x.StudentUserId == userId);
        }

        public IDataResult<InternshipsBook> GetFirstByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<InternshipsBookPageListDto>> GetPageListDtoByStudentId(int studentUserId)
        {
            var pages = _repository.GetAll(x=>x.StudentUserId == studentUserId).OrderBy(x=>x.Date).ToList();
            if (pages.Count() < 1)
                return new ErrorDataResult<List<InternshipsBookPageListDto>>("Henuz veri girilmemis!");
            var mappedPages = _mapper.Map<List<InternshipsBookPageListDto>>(pages);

            return new SuccessDataResult<List<InternshipsBookPageListDto>>(mappedPages);

        }

        public IDataResult<InternshipsBook> GetWithImages(int id)
        {
            var result = BusinessRules.Run(IsBookExist(id));
            if (result == null)
            {
                var data = _repository.Get(x => x.Id == id, x=>x.BookImages);
                return new SuccessDataResult<InternshipsBook>(data);
            }

            return new ErrorDataResult<InternshipsBook>(result.Message);
        }

        public IResult HardDelete(InternshipsBook entity)
        {
            var result = BusinessRules.Run(IsBookExist(entity.Id));
            if(result == null)
            {
                _repository.Delete(entity);
                return new SuccessResult();
            }
            return result;
        }

        public IResult HardDelete(int id)
        {
            var result = BusinessRules.Run(IsBookExist(id));
            if(result == null)
            {
                var data = _repository.Get(x=>x.Id == id);
                _repository.Delete(data);
                return new SuccessResult();
            }

            return result;
        }
        public IDataResult<InternshipsBook> Update(InternshipsBookPageUpdateDto entity)
        {
            var result = BusinessRules.Run(IsBookExist(entity.Id));
            if( result == null)
            {
                var mappedData = _mapper.Map<InternshipsBook>(entity);
                var updatedData = _repository.Update(mappedData);
                return new SuccessDataResult<InternshipsBook>(updatedData, "Güncelleme işlemi başarılı!");
            }

            return new ErrorDataResult<InternshipsBook>(result.Message);
        }

        private IResult IsBookExist(int id)
        {
            var result = _repository.Any(x=>x.Id ==id);
            if (result)
            {
                return new SuccessResult();
            }
            return new ErrorResult(Messages.IsExist);
        }

        private IResult CheckBookCountByStudentId(int studentId)
        {
            var result = _repository.Count(x=>x.StudentUserId == studentId);
            if (result >= 40)
                return new ErrorResult($"Maksimum 40 sayfa ekleyebilirsiniz!");

            return new SuccessResult();
        }
    }
}
