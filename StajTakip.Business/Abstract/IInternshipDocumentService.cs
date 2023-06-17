using Core.Utilities.Results;
using StajTakip.Entities.ComplexTypes;
using StajTakip.Entities.Concrete;

namespace StajTakip.Business.Abstract
{
    public interface IInternshipDocumentService
    {
        IResult Add(InternshipDocument entity);
        IResult HardDelete(InternshipDocument entity);
        IResult HardDelete(int id);
        IDataResult<InternshipDocument> Update(InternshipDocument entity);
        IDataResult<InternshipDocument> SignDocument(InternshipDocument entity, int userId, Roles role);
        IDataResult<InternshipDocument> RejectDocument(InternshipDocument entity, int adminUserId);
        IDataResult<InternshipDocument> AcceptDocument(InternshipDocument entity, int adminUserId);
        IDataResult<InternshipDocument> Get(int id);
        IDataResult<List<InternshipDocument>> GetAll();
        IDataResult<List<InternshipDocument>> GetAllByStudentId(int studentId);
    }
}
