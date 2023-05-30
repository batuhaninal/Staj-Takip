using Core.Utilities.Results;
using StajTakip.Entities.Concrete;
using StajTakip.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Abstract
{
    public interface IInternshipsBookService
    {
        IResult Add(InternshipsBookPageAddDto entity);
        IResult CheckBook(CheckBookDto entity);
        IResult HardDelete(InternshipsBook entity);
        IResult HardDelete(int id);
        IDataResult<InternshipsBook> Update(InternshipsBookPageUpdateDto entity);
        IDataResult<List<InternshipsBookPageListDto>> GetPageListDtoByStudentId(int studentUserId);
        IDataResult<InternshipsBook> Get(int id);
        IDataResult<InternshipsBook> GetFirstByUserId(int userId);
        IDataResult<List<InternshipsBook>> GetAll();
        IDataResult<List<InternshipsBook>> GetAllPagesWithImagesAndSignatures(int userId);
    }
}
