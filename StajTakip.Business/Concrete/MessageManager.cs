using Core.Utilities.Business;
using Core.Utilities.Results;
using StajTakip.Business.Abstract;
using StajTakip.Business.Constants;
using StajTakip.DataAccess.Abstract;
using StajTakip.Entities.Concrete;
using StajTakip.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Concrete
{
    public class MessageManager : IMessageService
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IMailService _mailService;
        private readonly IStudentUserService _studentUserService;
        private readonly IAdminStudentRelationService _adminStudentRelationService;
        private readonly IAdminUserService _adminUserService;
        private readonly IUserService _userService;

        public MessageManager(IMessageRepository messageRepo, IMailService mailService, IUserService userService, IStudentUserService studentUserService, IAdminStudentRelationService adminStudentRelationService, IAdminUserService adminUserService)
        {
            _messageRepo = messageRepo;
            _mailService = mailService;
            _studentUserService = studentUserService;
            _adminStudentRelationService = adminStudentRelationService;
            _adminUserService = adminUserService;
            _userService = userService;
        }

        public IResult Delete(int id)
        {
            var result = BusinessRules.Run(CheckMessage(id));
            if (result == null)
            {
                var message = _messageRepo.Get(x => x.Id == id);
                _messageRepo.Delete(message);
                return new SuccessResult();
            }
            return new ErrorResult(result.Message);
        }

        public IDataResult<List<Message>> GetAll()
        {
            var messages = _messageRepo.GetAll().ToList();
            return new SuccessDataResult<List<Message>>(messages);
        }

        public IDataResult<Message> GetByMessageId(int messageId)
        {
            var result = BusinessRules.Run(CheckMessage(messageId));
            if (result == null)
            {
                var message = _messageRepo.Get(x => x.Id == messageId);
                return new SuccessDataResult<Message>(message);
            }
            return new ErrorDataResult<Message>(result.Message);
        }

        public IDataResult<List<Message>> GetInboxListByUser(int receiverId)
        {
            var messages = _messageRepo.GetAll(x => x.ReceiverUserId == receiverId, x => x.SenderUser).ToList();
            if (messages == null)
                return new ErrorDataResult<List<Message>>("Mesaj kutusu boş");

            return new SuccessDataResult<List<Message>>(messages);
        }

        public IDataResult<List<Message>> GetSendboxListByUser(int senderId)
        {
            var messages = _messageRepo.GetAll(x => x.SenderUserId == senderId, x => x.ReceiverUser).ToList();
            if (messages == null)
                return new ErrorDataResult<List<Message>>("Mesaj kutusu boş");

            return new SuccessDataResult<List<Message>>(messages);
        }

        public IResult Update(Message message)
        {
            var result = BusinessRules.Run(CheckEntity(message), CheckMessage(message.Id));
            if (result == null)
            {
                _messageRepo.Update(message);
                return new SuccessResult();
            }
            return new ErrorResult(result.Message);
        }

        private IResult CheckMessage(int messageId)
        {
            var message = _messageRepo.Get(x => x.Id == messageId);
            if (message == null)
                return new ErrorResult("Message bulunamadi!");

            return new SuccessResult();
        }

        private IResult CheckEntity(Message entity)
        {
            if (entity == null)
                return new ErrorResult("Bos nesne!");
            return new SuccessResult();
        }

        public IResult SendTemplateIssue(int studentUserId)
        {
            var user = _studentUserService.GetByIdWithRelations(studentUserId);
            if (!user.Success)
                return new ErrorResult(user.Message);

            var relationResult = _adminStudentRelationService.GetAllByStudentIdWithAdmin(user.Data.Id);

            if (!relationResult.Success)
                return new ErrorResult(relationResult.Message ?? "Beklenmeyen bir hata ile karşılaşıldı!");

            foreach (var relation in relationResult.Data)
            {
                if (!relation.IsCompany)
                {
                    Message message = Messages.TempIssue();

                    message.SenderUserId = user.Data.UserId;
                    message.ReceiverUserId = relation.AdminUser.UserId;
                    _messageRepo.Add(message);
                }
            }

            return new SuccessResult();
        }

        public IDataResult<List<Message>> GetLastNotificationListByUser(int receiverId)
        {
            var user = _adminUserService.GetByAdminUserId(receiverId);
            if (!user.Success)
                return new ErrorDataResult<List<Message>>("Beklenmeyen bir hata meydana geldi!");
            var messages = _messageRepo.GetAll(x => x.ReceiverUserId == user.Data.UserId, x => x.SenderUser).TakeLast(5).ToList();
            if (messages == null)
                return new ErrorDataResult<List<Message>>("Mesaj kutusu boş");

            return new SuccessDataResult<List<Message>>(messages);
        }

        public IDataResult<List<Message>> GetLastNotificationListByCompany(int receiverId)
        {
            var user = _adminUserService.GetByAdminUserId(receiverId);
            if (!user.Success)
                return new ErrorDataResult<List<Message>>("Beklenmeyen bir hata meydana geldi!");
            var messages = _messageRepo.GetAll(x => x.ReceiverUserId == user.Data.UserId, x => x.SenderUser).TakeLast(5).ToList();
            if (messages == null)
                return new ErrorDataResult<List<Message>>("Mesaj kutusu boş");

            return new SuccessDataResult<List<Message>>(messages);
        }
        public IResult SendDocumentAdded(int studentUserId, int documentId, string documentName)
        {
            var user = _studentUserService.GetById(studentUserId);
            if (!user.Success)
                return new ErrorResult(user.Message);

            var relationResult = _adminStudentRelationService.GetAllByStudentIdWithAdmin(user.Data.Id);

            if (!relationResult.Success)
                return new ErrorResult(relationResult.Message ?? "Beklenmeyen bir hata ile karşılaşıldı!");

            int[] ids = new int[relationResult.Data.Count+1];
            int i = 0;
            string name = user.Data.FirstName + " " + user.Data.LastName;
            foreach (var relation in relationResult.Data)
            {

                Message message = Messages.StudentAddedDocument(name,documentId,documentName);
                message.SenderUserId = user.Data.UserId;
                message.ReceiverUserId = relation.AdminUser.UserId;
                _messageRepo.Add(message);
                ids[i] = relation.AdminUser.UserId;
                i++;
            }

            ids[i] = user.Data.UserId;

            var emails = _userService.GetEmailsByIds(ids);

            if (emails.Success)
            {
                var mail = new EmailSendDto
                {
                    Subject = Messages.StudentAddedDocument(name, documentId, documentName).Subject,
                    Message = Messages.StudentAddedDocument(name, documentId, documentName).MessageDetail,
                    SenderMail = emails.Data[0],
                    ReceiverMail = emails.Data
                };

                var sendMail = _mailService.Send(mail);
            }

            return new SuccessResult();
        }
    }
}
