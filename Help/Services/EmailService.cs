using Help.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Help.Services
{
    public class EmailService:Singleton<EmailService>,IEmailService
    {
        private IAppSetting _appSetting;
        public EmailService(IAppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        public void Send(string receiver, string subject, string bodyText)
        {
            using(SmtpClient smtp=new SmtpClient(_appSetting.EmailHost,_appSetting.EmailPort))
            {
                MailAddress from = new MailAddress(_appSetting.SenderAccount);
                MailAddress to = new MailAddress(receiver);

                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(from.Address, _appSetting.SenderAccountPassword);
                smtp.Timeout = 20000;

                MailMessage message = new MailMessage(from, to);
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.Subject = subject;
                message.BodyEncoding= System.Text.Encoding.UTF8;
                message.Body = bodyText;
                smtp.Send(message);
                message.Dispose();
            }
        }
    }
}
