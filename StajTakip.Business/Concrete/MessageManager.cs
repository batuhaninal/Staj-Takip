using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using StajTakip.Business.Abstract;
using StajTakip.Business.Constants;
using StajTakip.DataAccess.Abstract;
using StajTakip.Entities.ComplexTypes;
using StajTakip.Entities.Concrete;
using StajTakip.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            var messages = _messageRepo.GetAll(x => x.ReceiverUserId == user.Data.UserId && !x.IsTeacherRead, x => x.SenderUser).TakeLast(5).ToList();
            if (messages == null)
                return new ErrorDataResult<List<Message>>("Mesaj kutusu boş");

            return new SuccessDataResult<List<Message>>(messages);
        }

        public IDataResult<List<Message>> GetLastNotificationListByCompany(int receiverId)
        {
            var user = _adminUserService.GetByAdminUserId(receiverId);
            if (!user.Success)
                return new ErrorDataResult<List<Message>>("Beklenmeyen bir hata meydana geldi!");
            var messages = _messageRepo.GetAll(x => x.ReceiverUserId == user.Data.UserId && !x.IsCompanyRead, x => x.SenderUser).TakeLast(5).ToList();
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

        public IResult RejectDocument(int studentId, int adminId, int documentId, string documentName)
        {
            var studentUser = _studentUserService.GetById(studentId);
            var adminUser = _adminUserService.GetByAdminUserId(adminId);
            if (!studentUser.Success)
                return new ErrorResult(studentUser.Message ?? "Beklenmeyen bir hata meydana geldi!");

            if(!adminUser.Success)
                return new ErrorResult(adminUser.Message ?? "Beklenmeyen bir hata meydana geldi!");

            var message = Messages.RejectDocument($"{studentUser.Data.FirstName} {studentUser.Data.LastName}", $"{adminUser.Data.FirstName} {adminUser.Data.LastName}", documentId, documentName);

            message.SenderUserId = adminUser.Data.UserId;
            message.ReceiverUserId = studentUser.Data.UserId;

            _messageRepo.Add(message);
            return new SuccessResult();
        }


        public IResult AcceptDocument(int studentId, int adminId, int documentId, string documentName)
        {
            var studentUser = _studentUserService.GetById(studentId);
            var adminUser = _adminUserService.GetByAdminUserId(adminId);
            if (!studentUser.Success)
                return new ErrorResult(studentUser.Message ?? "Beklenmeyen bir hata meydana geldi!");

            if (!adminUser.Success)
                return new ErrorResult(adminUser.Message ?? "Beklenmeyen bir hata meydana geldi!");

            var message = Messages.AcceptDocument($"{adminUser.Data.FirstName} {adminUser.Data.LastName}", documentId, documentName);

            message.SenderUserId = adminUser.Data.UserId;
            message.ReceiverUserId = studentUser.Data.UserId;

            _messageRepo.Add(message);
            return new SuccessResult();
        }

        public IResult SendSignedDocumentNoty(int senderStudentOrAdminId, int documentId, int documentOwnerId, string documentName, Roles role)
        {
            int[] ids;
            string name = "";
            // Student Notification
            if (role == Roles.Student)
            {
                var relationResult = _adminStudentRelationService.GetAllByStudentIdWithAdmin(senderStudentOrAdminId);

                if (!relationResult.Success)
                    return new ErrorResult(relationResult.Message ?? "Beklenmeyen bir hata ile karşılaşıldı!");

                ids = new int[relationResult.Data.Count + 1];
                int i = 0;
                int userId = 0; 
                foreach (var relation in relationResult.Data)
                {
                    if(i == 0)
                    {
                        userId = relation.StudentUser.UserId;
                        name = $"{relation.StudentUser.FirstName} {relation.StudentUser.LastName}";
                    }
                    Message message = Messages.SignedDocument(name, documentId, documentName);
                    message.SenderUserId = relation.StudentUser.UserId;
                    message.ReceiverUserId = relation.AdminUser.UserId;
                    _messageRepo.Add(message);
                    ids[i] = relation.AdminUser.UserId;
                    i++;
                }

                ids[i] = userId;
            }
            // Admin ve company notification
            else
            {
                var user = _adminUserService.GetByAdminUserId(senderStudentOrAdminId);
                if (!user.Success)
                    return new ErrorResult(user.Message ?? "Beklenmeyen hata!");
                name = $"{user.Data.FirstName} {user.Data.LastName}";
                Message adminMessage = Messages.SignedDocument(name, documentId, documentName);
                adminMessage.SenderUserId = user.Data.UserId;
                var studentUser = _studentUserService.GetById(documentOwnerId);
                if (!studentUser.Success)
                    return new ErrorResult(studentUser.Message ?? "Beklenmeyen hata!");
                adminMessage.ReceiverUserId = studentUser.Data.UserId;
                _messageRepo.Add(adminMessage);
                ids = new int[2];
                ids[0] = studentUser.Data.UserId;
                ids[1] = user.Data.UserId;
            }

            //E-Posta
            var emails = _userService.GetEmailsByIds(ids);

            if (emails.Success)
            {
                var mail = new EmailSendDto
                {
                    Subject = Messages.SignedDocument(name, documentId, documentName).Subject,
                    Message = Messages.SignedDocument(name, documentId, documentName).MessageDetail,
                    SenderMail = emails.Data[0],
                    ReceiverMail = emails.Data
                };

                var sendMail = _mailService.Send(mail);
                if (sendMail.Success)
                    return new SuccessResult("Bildirim gönderildi fakat e-posta gönderilemedi!" + sendMail.Message ?? "E-Posta hatas!");
            }
            return new SuccessResult("Bildirim gönderildi fakat e-posta gönderilemedi!");
        }

        public int GetInboxNewMessageCount(int receiverId, Roles role)
        {
            int count = 0;
            if (role == Roles.Student)
            {
                var sUserId = _studentUserService.GetById(receiverId).Data.UserId;
                count = _messageRepo.Count(x => x.ReceiverUserId == sUserId && !x.IsStudentRead);
            }
            else
            {
                var aUserId = _adminUserService.GetByAdminUserId(receiverId).Data.UserId;
                if (role == Roles.Company)
                {
                    count = _messageRepo.Count(x => x.ReceiverUserId == aUserId && !x.IsCompanyRead);
                }
                else if (role == Roles.Teacher)
                {
                    count = _messageRepo.Count(x => x.ReceiverUserId == aUserId && !x.IsTeacherRead);
                }
            }    
            return count;
        }
    }
}
