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
    public class SignatureManager : ISignatureService
    {
        private readonly ISignatureRepository _signatureRepository;

        public SignatureManager(ISignatureRepository signatureRepository)
        {
            _signatureRepository = signatureRepository;
        }

        public IResult Add(Signature entity)
        {
            _signatureRepository.Add(entity);
            return new SuccessResult();
        }

        public IDataResult<Signature> Get(int id)
        {
            var result = BusinessRules.Run(IsExist(id));
            if (result == null)
            {
                var data = _signatureRepository.Get(x => x.Id == id);
                return new SuccessDataResult<Signature>(data);
            }

            return new ErrorDataResult<Signature>(result.Message);
        }

        public IDataResult<List<Signature>> GetAll()
        {
            var data = _signatureRepository.GetAll().ToList();
            return new SuccessDataResult<List<Signature>>(data);

        }

        public IResult HardDelete(Signature entity)
        {
            var result = BusinessRules.Run(IsExist(entity.Id));
            if (result == null)
            {
                _signatureRepository.Delete(entity);
                return new SuccessResult();
            }

            return result;
        }

        public IResult HardDelete(int id)
        {
            var result = BusinessRules.Run(IsExist(id));
            if (result == null)
            {
                var entity = _signatureRepository.Get(x => x.Id ==id);
                _signatureRepository.Delete(entity);
                return new SuccessResult();
            }

            return result;
        }

        public IDataResult<Signature> Update(Signature entity)
        {
            var result = BusinessRules.Run(IsExist(entity.Id));
            if (result == null)
            {
                var data = _signatureRepository.Update(entity);
                return new SuccessDataResult<Signature>(data);
            }

            return new ErrorDataResult<Signature>(result.Message);
        }

        private IResult IsExist(int id)
        {
            var result = _signatureRepository.Any(x=>x.Id ==  id);
            if (result)
            {
                return new SuccessResult();
            }

            return new ErrorResult(Messages.IsExist);
        }
    }
}
