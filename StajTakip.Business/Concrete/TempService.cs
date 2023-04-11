using Core.Utilities.Business;
using Core.Utilities.Results;
using StajTakip.Business.Abstract;
using StajTakip.DataAccess.Abstract;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Concrete
{
    public class TempService : ITempService
    {
        private readonly ITempRepository _tempRepository;
        private readonly ITempRepositoryAsync _tempRepositoryAsync;

        public TempService(ITempRepository tempRepository, ITempRepositoryAsync tempRepositoryAsync)
        {
            _tempRepository = tempRepository;
            _tempRepositoryAsync = tempRepositoryAsync;
        }

        public IResult Add(Temp temp)
        {
            var result = BusinessRules.Run(CheckDemo1(temp), CheckDem2(temp));
            if (result != null)
            {
                return new ErrorResult(result.Message);
            }
            _tempRepository.Add(temp);
            return new SuccessResult();
        }

        public async Task AddAsync(Temp temp)
        {
            await _tempRepositoryAsync.AddAsync(temp);
        }

        public void Delete(int tempId)
        {
            var temp = _tempRepository.Get(x=>x.Id == tempId);
            _tempRepository.Delete(temp);
        }

        public async Task DeleteAsync(int tempId)
        {
            var temp = await _tempRepositoryAsync.GetAsync(x=> x.Id == tempId);
            await _tempRepositoryAsync.DeleteAsync(temp);
        }

        public IList<Temp> GetAll()
        {
            return _tempRepository.GetAll().ToList();
        }

        public async Task<IList<Temp>> GetAllAsync()
        {
            var x = await _tempRepositoryAsync.GetAllAsync();
            return x.ToList();
        }

        public Temp GetById(int id)
        {
            return _tempRepository.Get(x => x.Id == id);
        }

        public async Task<Temp> GetByIdAsync(int id)
        {
            return await _tempRepositoryAsync.GetAsync(x => x.Id == id);    
        }

        public void Update(Temp temp)
        {
            _tempRepository.Update(temp);
        }

        public async Task UpdateAsync(Temp temp)
        {
            await _tempRepositoryAsync.UpdateAsync(temp);
        }

        private IResult CheckDemo1(Temp temp)
        {
            if(temp == null)
            {
                return new ErrorResult("Alanlar bos!");
            }
            return new SuccessResult();
        }

        private IResult CheckDem2(Temp temp)
        {
            if(temp.Price < 15)
            {
                return new ErrorResult("Fiyat 15den kucuk olamaz");
            }
            return new SuccessResult();
        }
    }
}
