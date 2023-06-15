using Core.Utilities.Results;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Abstract
{
    public interface IMessageService
    {
        IDataResult<List<Message>> GetInboxListByUser(int receiverId);
        IDataResult<List<Message>> GetLastNotificationListByUser(int receiverId);
        IDataResult<List<Message>> GetLastNotificationListByCompany(int receiverId);
        IDataResult<List<Message>> GetSendboxListByUser(int senderId);
        IResult SendTemplateIssue(int studentUserId);
        IResult SendDocumentAdded(int studentUserId, int documentId, string documentName);
        IResult Delete(int id);
        IResult Update(Message message);
        IDataResult<Message> GetByMessageId(int messageId);
        IDataResult<List<Message>> GetAll();
    }
}
