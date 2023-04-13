using Core.Utilities.Results;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Abstract
{
    public interface IBookImageService
    {
        IResult Add(BookImage entity);
        IResult HardDelete(BookImage entity);
        IResult HardDelete(int id);
        IDataResult<BookImage> Update(BookImage entity);
        IDataResult<BookImage> Get(int id);
        IDataResult<List<BookImage>> GetAll();
    }
}
