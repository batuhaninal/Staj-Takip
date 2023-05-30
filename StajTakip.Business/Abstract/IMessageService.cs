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
        IDataResult<List<Message>> GetSendboxListByUser(int senderId);
        IResult Add(Message message);
        IResult Delete(int id);
        IResult Update(Message message);
        IDataResult<Message> GetByMessageId(int messageId);
        IDataResult<Message> GetByMessageIdWithUsers(int messageId);
        IDataResult<List<Message>> GetAll();
    }
}
