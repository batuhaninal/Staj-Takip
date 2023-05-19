using Core.Utilities.Results;
using StajTakip.Entities.Concrete;

namespace StajTakip.Business.Abstract
{
    public interface IInternshipDocumentService
    {
        IResult Add(InternshipDocument entity);
        IResult HardDelete(InternshipDocument entity);
        IResult HardDelete(int id);
        IDataResult<InternshipDocument> Update(InternshipDocument entity);
        IDataResult<InternshipDocument> Get(int id);
        IDataResult<List<InternshipDocument>> GetAll();
    }
}
