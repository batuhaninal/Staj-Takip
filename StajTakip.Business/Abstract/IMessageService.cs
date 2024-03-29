﻿using Core.Utilities.Results;
using StajTakip.Entities.ComplexTypes;
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
        IResult SendSignedDocumentNoty(int senderStudentOrAdminId, int documentId, int documentOwnerId, string documentName, Roles role);
        IResult DeleteRelation(int relationId,int teacherId);
        IResult AddRelation(int teacherId, int studentId, int companyId);
        IResult RejectDocument(int studentId, int adminId, int documentId, string documentName);
        IResult AcceptDocument(int documentId, string documentName, string adminFullName, string studentNumber, int senderId, int receiverId);
        IResult Delete(int id);
        IResult Update(Message message);
        IDataResult<Message> GetByMessageId(int messageId);
        IDataResult<List<Message>> GetAll();
        IResult SendFinish(int studentId);
        IResult ReplyFinish(int adminId, int studentId, bool? finish);
        int GetInboxNewMessageCount(int receiverId, Roles role);
    }
}
