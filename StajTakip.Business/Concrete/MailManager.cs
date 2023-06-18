using Core.Utilities.Results;
using Microsoft.Extensions.Options;
using StajTakip.Business.Abstract;
using StajTakip.Entities.Concrete;
using StajTakip.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Concrete
{
    public class MailManager : IMailService
    {
        private readonly SmtpSettings _smtpSettings;

        public MailManager(IOptions<SmtpSettings> smtpSettings)
        {
            // appsettingsten degerler bind edildi nesneye
            _smtpSettings = smtpSettings.Value;
        }

        public IResult Send(EmailSendDto emailSendDto)
        {
            try
            {
                MailMessage message = new MailMessage
                {
                    // stajtakip@outlook.com
                    From = new MailAddress(_smtpSettings.SenderEmail),
                    Subject = emailSendDto.Subject,
                    IsBodyHtml = true,
                    Body = $"Gönderen E-Posta: {emailSendDto.SenderMail}<br/>{emailSendDto.Message}",
                };

                for (int i = 0; i < emailSendDto.ReceiverMail.Count(); i++)
                {
                    if (i < (emailSendDto.ReceiverMail.Count() - 1))
                        message.To.Add(new MailAddress(emailSendDto.ReceiverMail[i]));
                }

                SmtpClient smtpClient = new SmtpClient
                {
                    Host = _smtpSettings.Server,
                    Port = _smtpSettings.Port,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                smtpClient.Send(message);

                return new SuccessResult("E-Posta basariyla gonderildi");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"{ex.Message}");
            }

            
        }

        public IResult SendForgotPasswordInfo(ForgotPasswordEmailDto model)
        {
            try
            {
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail),
                    To = { new MailAddress(model.ReceiverMail) },
                    Subject = "Şifre Değişimi Başarıyla Gerçekleşti!",
                    IsBodyHtml = true,
                    Body = $"Yeni Şifreniz : {model.Password} <br/>" +
                $"Lütfen giriş yaptıktan sonra şifrenizi değiştiriniz! <br/>" +
                $"Gönderen : {_smtpSettings.SenderName}<br/>" +
                $"Gönderen Email :{_smtpSettings.SenderEmail}<br/>",
                };

                SmtpClient smtpClient = new SmtpClient
                {
                    Host = _smtpSettings.Server,
                    Port = _smtpSettings.Port,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                smtpClient.Send(message);

                return new SuccessResult("E-Posta basariyla gonderildi");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"{ex.Message}");
            }
            
        }
    }
}
