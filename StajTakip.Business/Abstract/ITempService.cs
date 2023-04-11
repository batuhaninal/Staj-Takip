using Core.Utilities.Results;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Abstract
{
    public interface ITempService
    {
        IList<Temp> GetAll();
        Task<IList<Temp>> GetAllAsync();
        Temp GetById(int id);
        Task<Temp> GetByIdAsync(int id);
        IResult Add(Temp temp);
        Task AddAsync(Temp temp);
        void Update(Temp temp);
        Task UpdateAsync(Temp temp);
        void Delete(int tempId);
        Task DeleteAsync(int tempId);
    }
}
