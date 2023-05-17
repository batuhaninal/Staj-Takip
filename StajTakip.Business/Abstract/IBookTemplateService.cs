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
    public interface IBookTemplateService
    {
        IResult Add(BookTemplate entity);
        IResult HardDelete(int id);
        IDataResult<BookTemplate> Update(BookTemplate entity);
        IDataResult<BookTemplate> Get(int id);
        IDataResult<BookTemplate> GetCurrent();
        IDataResult<List<BookTemplate>> GetAll();
    }
}
