using Core.Utilities.Results;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Abstract
{
    
    public interface ISignatureService
    {
        IResult Add(Signature entity);
        IResult HardDelete(Signature entity);
        IResult HardDelete(int id);
        IDataResult<Signature> Update(Signature entity);
        IDataResult<Signature> Get(int id);
        IDataResult<List<Signature>> GetAll();
    }
}
