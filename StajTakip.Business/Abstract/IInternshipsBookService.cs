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
        IResult Add(InternshipsBook entity);
        IResult HardDelete(InternshipsBook entity);
        IResult HardDelete(int id);
        IDataResult<InternshipsBook> Update(InternshipsBook entity);
        IDataResult<List<InternshipsBook>> GetAllPages(int userId);
        IDataResult<List<InternshipsBookPageListDto>> GetPages();
        IDataResult<InternshipsBook> Get(int id);
        IDataResult<List<InternshipsBook>> GetAll();
        IDataResult<List<InternshipsBook>> GetAllPagesWithImagesAndSignatures(int userId);
    }
}
