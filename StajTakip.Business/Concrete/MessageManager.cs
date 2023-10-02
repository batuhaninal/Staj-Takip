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
using System.Reflection.Metadata;
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
        private readonly RabbitMQPublisher _rabbitMQPublisher;

        public MessageManager(IMessageRepository messageRepo, IMailService mailService, IUserService userService, IStudentUserService studentUserService, IAdminStudentRelationService adminStudentRelationService, IAdminUserService adminUserService, RabbitMQPublisher rabbitMQPublisher)
        {
            _messageRepo = messageRepo;
            _mailService = mailService;
            _studentUserService = studentUserService;
            _adminStudentRelationService = adminStudentRelationService;
            _adminUserService = adminUserService;
            _userService = userService;
            _rabbitMQPublisher = rabbitMQPublisher;
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
            var messages = _messageRepo.GetAll(x => x.ReceiverUserId == user.Data.UserId && !x.IsTeacherRead, x => x.SenderUser).OrderBy(x=>x.MessageDate).TakeLast(5).ToList();
            if (messages == null)
                return new ErrorDataResult<List<Message>>("Mesaj kutusu boş");

            return new SuccessDataResult<List<Message>>(messages);
        }

        public IDataResult<List<Message>> GetLastNotificationListByCompany(int receiverId)
        {
            var user = _adminUserService.GetByAdminUserId(receiverId);
            if (!user.Success)
                return new ErrorDataResult<List<Message>>("Beklenmeyen bir hata meydana geldi!");
            var messages = _messageRepo.GetAll(x => x.ReceiverUserId == user.Data.UserId && !x.IsCompanyRead, x => x.SenderUser).OrderBy(x=>x.MessageDate).TakeLast(5).ToList();
            if (messages == null)
                return new ErrorDataResult<List<Message>>("Mesaj kutusu boş");

            return new SuccessDataResult<List<Message>>(messages);
        }
        public IResult SendDocumentAdded(int studentUserId, int documentId, string documentName)
        {
            var user = _studentUserService.GetByIdWithUser(studentUserId);
            if (!user.Success)
                return new ErrorResult(user.Message);

            var relationResult = _adminStudentRelationService.GetAllByStudentIdWithAdmin(user.Data.Id);

            if (!relationResult.Success)
                return new ErrorResult(relationResult.Message ?? "Beklenmeyen bir hata ile karşılaşıldı!");

            int[] ids = new int[relationResult.Data.Count+1];
            int i = 0;
            string name = user.Data.FirstName + " " + user.Data.LastName;
            string studentNumber = user.Data.StudentNumber;
            foreach (var relation in relationResult.Data)
            {

                var message = Messages.StudentAddedDocument(name,documentId,documentName, studentNumber);
                message.SenderUserId = user.Data.UserId;
                message.ReceiverUserId = relation.AdminUser.UserId;
                //_messageRepo.Add(message);
                _rabbitMQPublisher.Publish(message);
                ids[i] = relation.AdminUser.UserId;
                i++;
            }

            ids[i] = user.Data.UserId;

            var emails = _userService.GetEmailsByIds(ids);

            if (emails.Success)
            {
                var mail = new CreatedMailMessage
                {
                    Subject = Messages.StudentAddedDocument(name, documentId, documentName, studentNumber).Subject,
                    MessageBody = Messages.StudentAddedDocument(name, documentId, documentName, studentNumber).MessageDetail,
                    SenderMail = user.Data.User.Email,
                    ReceiverMail = emails.Data
                };

                //var sendMail = _mailService.Send(mail);

                _rabbitMQPublisher.Publish(mail);
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

            var message = Messages.RejectDocument($"{studentUser.Data.FirstName} {studentUser.Data.LastName}", $"{adminUser.Data.FirstName} {adminUser.Data.LastName}", documentId, documentName, studentUser.Data.StudentNumber);

            message.SenderUserId = adminUser.Data.UserId;
            message.ReceiverUserId = studentUser.Data.UserId;

            _messageRepo.Add(message);
            return new SuccessResult();
        }


        public IResult AcceptDocument(int documentId, string documentName, string adminFullName, string studentNumber, int senderId, int receiverId)
        {
            //var studentUser = _studentUserService.GetById(studentId);
            //var adminUser = _adminUserService.GetByAdminUserId(adminId);
            //if (!studentUser.Success)
            //    return new ErrorResult(studentUser.Message ?? "Beklenmeyen bir hata meydana geldi!");

            //if (!adminUser.Success)
            //    return new ErrorResult(adminUser.Message ?? "Beklenmeyen bir hata meydana geldi!");

            var message = Messages.AcceptDocument($"{adminFullName}", documentId, documentName, studentNumber);

            message.SenderUserId = senderId;
            message.ReceiverUserId = receiverId;

            //_messageRepo.Add(message);
            _rabbitMQPublisher.Publish(message);
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

        public IResult DeleteRelation(int relationId, int teacherId)
        {
            var relation = _adminStudentRelationService.Get(relationId);
            if (!relation.Success)
                return new ErrorResult(relation.Message ?? "Beklenmeyen hata!");

            var studentUser = _studentUserService.GetByIdWithUser(relation.Data.StudentUserId.Value);
            var companyUser = _adminUserService.GetByAdminUserIdWithUser(relation.Data.AdminUserId.Value);
            var teacherUser = _adminUserService.GetByAdminUserIdWithUser(teacherId);

            var relationResult = _adminStudentRelationService.HardDelete(relation.Data);
            if (!relationResult.Success)
                return new ErrorResult(relationResult.Message ?? "Beklenmeyen hata!");


            if (!studentUser.Success || !companyUser.Success || !teacherUser.Success)
                return new SuccessResult($"Öğrencinin ilişkisi silindi fakat mail gönderilemedi!");

            var studentName = $"{studentUser.Data.FirstName} {studentUser.Data.LastName}";
            var companyName = $"{companyUser.Data.FirstName} {companyUser.Data.LastName}";
            var teacherName = $"{teacherUser.Data.FirstName} {teacherUser.Data.LastName}";
            for (int i = 0; i < 2; i++)
            {
                var message = Messages.DeleteRelation(teacherName, studentName, companyName);
                message.SenderUserId = teacherUser.Data.UserId;
                if (i == 0)
                {
                    message.ReceiverUserId = companyUser.Data.UserId;
                }
                else
                {
                    message.ReceiverUserId = studentUser.Data.UserId;
                }
                _messageRepo.Add(message);
            }

            string[] emails = { studentUser.Data.User.Email, companyUser.Data.User.Email, teacherUser.Data.User.Email };

            var mail = new EmailSendDto
            {
                Subject = Messages.DeleteRelation(teacherName, studentName, companyName).Subject,
                Message = Messages.DeleteRelation(teacherName, studentName, companyName).MessageDetail,
                SenderMail = emails[2],
                ReceiverMail = emails
            };

            var mailResult = _mailService.Send(mail);
            if (!mailResult.Success)
                return new SuccessResult(mailResult.Message ?? "E-Posta gönderilirken hata oluştu ama bildirim gönderildi");

            return new SuccessResult();
        }

        public IResult AddRelation(int teacherId, int studentId, int companyId)
        {
            var relationResult = _adminStudentRelationService.AddForCompany(new AdminStudentRelation
            {
                StudentUserId = studentId,
                AdminUserId = companyId,
            });

            if (!relationResult.Success)
                return new ErrorResult(relationResult.Message ?? "Beklenmeyen hata!");

            var studentUser = _studentUserService.GetByIdWithUser(studentId);
            var companyUser = _adminUserService.GetByAdminUserIdWithUser(companyId);
            var teacherUser = _adminUserService.GetByAdminUserIdWithUser(teacherId);
            if (!studentUser.Success || !companyUser.Success || !teacherUser.Success)
                return new SuccessResult($"Öğrencinin ilişkisi eklendi fakat mail gönderilemedi!");

            var studentName = $"{studentUser.Data.FirstName} {studentUser.Data.LastName}";
            var companyName = $"{companyUser.Data.FirstName} {companyUser.Data.LastName}";
            var teacherName = $"{teacherUser.Data.FirstName} {teacherUser.Data.LastName}";
            for (int i = 0; i < 2; i++)
            {
                var message = Messages.AddRelation(teacherName, studentName, companyName);
                message.SenderUserId = teacherUser.Data.UserId;
                if (i == 0)
                {
                    message.ReceiverUserId = companyUser.Data.UserId;
                }
                else
                {
                    message.ReceiverUserId = studentUser.Data.UserId;
                }
                _messageRepo.Add(message);
            }

            string[] emails = { studentUser.Data.User.Email, companyUser.Data.User.Email, teacherUser.Data.User.Email };

            var mail = new EmailSendDto
            {
                Subject = Messages.AddRelation(teacherName, studentName, companyName).Subject,
                Message = Messages.AddRelation(teacherName, studentName, companyName).MessageDetail,
                SenderMail = emails[2],
                ReceiverMail = emails
            };

            var mailResult = _mailService.Send(mail);
            if (!mailResult.Success)
                return new SuccessResult(mailResult.Message ?? "E-Posta gönderilirken hata oluştu ama bildirim gönderildi");

            return new SuccessResult();
        }

        public IResult SendFinish(int studentId)
        {
            var user = _studentUserService.GetByIdWithUser(studentId);
            if (!user.Success)
                return new ErrorResult(user.Message ?? "Beklenmeyen hata!");

            user.Data.IsFinished = false;
            var updatedUser = _studentUserService.Update(user.Data);
            if(!updatedUser.Success)
                return new ErrorResult(updatedUser.Message ?? "Beklenmeyen hata!");

            var relationResult = _adminStudentRelationService.GetAllByStudentIdWithAdmin(studentId);
            if(!relationResult.Success)
                return new ErrorResult(relationResult.Message ?? "Beklenmeyen hata!");

            int[] ids = new int[relationResult.Data.Count + 1];
            int i = 0;
            string name = user.Data.FirstName + " " + user.Data.LastName;
            string studentNumber = user.Data.StudentNumber;
            foreach (var relation in relationResult.Data)
            {

                Message message = Messages.SendFinish(studentId,name, studentNumber);
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
                    Subject = Messages.SendFinish(studentId, name, studentNumber).Subject,
                    Message = Messages.SendFinish(studentId, name, studentNumber).MessageDetail,
                    SenderMail = emails.Data[0],
                    ReceiverMail = emails.Data
                };

                var sendMail = _mailService.Send(mail);
            }

            return new SuccessResult();


        }

        public IResult ReplyFinish(int adminId, int studentId, bool? finish)
        {
            var admin = _adminUserService.GetByAdminUserIdWithUser(adminId);
            var studentUser = _studentUserService.GetByIdWithUser(studentId);

            if (!admin.Success || !studentUser.Success)
                return new ErrorResult($"{admin.Message} {studentUser.Message}");

            studentUser.Data.IsFinished = finish;
            var updatedStudentUser = _studentUserService.Update(studentUser.Data);
            if(!updatedStudentUser.Success)
                return new ErrorResult(updatedStudentUser.Message ?? "Beklenmeyen Hata!");

            var adminName = $"{admin.Data.FirstName} {admin.Data.LastName}";
            var studentName = $"{studentUser.Data.FirstName} {studentUser.Data.LastName}";

            Message message;
            if (finish.HasValue)
                message = Messages.AcceptFinish(studentName, adminName);
            else
                message = Messages.RejectFinish(studentName, adminName);

            message.SenderUserId = admin.Data.UserId;
            message.ReceiverUserId = studentUser.Data.UserId;
            _messageRepo.Add(message);

            string[] emails = { studentUser.Data.User.Email, admin.Data.User.Email };
            var mail = new EmailSendDto
            {
                Subject = message.Subject,
                Message = message.MessageDetail,
                SenderMail = admin.Data.User.Email,
                ReceiverMail = emails
            };

            var mailResult = _mailService.Send(mail);

            return new SuccessResult();
        }
    }
}
