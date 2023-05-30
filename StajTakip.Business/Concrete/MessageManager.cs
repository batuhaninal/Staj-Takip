using Core.Utilities.Business;
using Core.Utilities.Results;
using StajTakip.Business.Abstract;
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

        public MessageManager(IMessageRepository messageRepo, IMailService mailService, IUserService userService)
        {
            _messageRepo = messageRepo;
            _mailService = mailService;
        }

        public IResult Add(Message message)
        {
            if (message == null)
                return new ErrorResult("Bos nesne!");
            _messageRepo.Add(message);

            var messageWUsers = GetByMessageIdWithUsers(message.Id);
            if (messageWUsers.Success)
            {
                var email = new EmailSendDto
                {
                    Message = message.MessageDetail,
                    SenderMail = messageWUsers.Data.SenderUser.Email,
                    Subject = message.Subject
                };
                _mailService.Send(email);
            }

            return new SuccessResult();
        }

        public IResult Delete(int id)
        {
            var result = BusinessRules.Run(CheckMessage(id));
            if(result == null)
            {
                var message = _messageRepo.Get(x=>x.Id == id);
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
            var messages = _messageRepo.GetAll(x => x.ReceiverId == receiverId, x => x.SenderUser).ToList();
            if (messages == null)
                return new ErrorDataResult<List<Message>>("Mesaj kutusu boş");

            return new SuccessDataResult<List<Message>>(messages);
        }

        public IDataResult<List<Message>> GetSendboxListByUser(int senderId)
        {
            var messages = _messageRepo.GetAll(x => x.SenderId == senderId, x => x.ReceiverUser).ToList();
            if (messages == null)
                return new ErrorDataResult<List<Message>>("Mesaj kutusu boş");

            return new SuccessDataResult<List<Message>>(messages);
        }

        public IResult Update(Message message)
        {
            var result = BusinessRules.Run(CheckEntity(message), CheckMessage(message.Id));
            if(result == null)
            {
                _messageRepo.Update(message);
                return new SuccessResult();
            }
            return new ErrorResult(result.Message);
        }

        private IResult CheckMessage(int messageId)
        {
            var message = _messageRepo.Get(x=>x.Id == messageId);
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

        public IDataResult<Message> GetByMessageIdWithUsers(int messageId)
        {
            var result = BusinessRules.Run(CheckMessage(messageId));
            if (result == null)
            {
                var message = _messageRepo.Get(x => x.Id == messageId, x=>x.ReceiverUser, x=>x.SenderUser);
                return new SuccessDataResult<Message>(message);
            }
            return new ErrorDataResult<Message>(result.Message);
        }
    }
}
