using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Services.Interfaces;

namespace Services
{
    public class EmailService:IEmailService
    {
        private readonly IConfigurationsProvider _configs;

        public EmailService(IConfigurationsProvider configs)
        {
            _configs = configs;
        }

        private SmtpClient GetSmtpClient()
        {
            var smtpSettings  = _configs.GetSmtpSettings();
            var client = new SmtpClient(smtpSettings.Host)
            {
                Credentials = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password),
                Port = smtpSettings.Port,
                EnableSsl = smtpSettings.EnableSsl
            };
            return client;
        }


        public bool SendEmial(string from, List<string> to, string subject, string body)
        {
            try
            {
                using (var client = GetSmtpClient())
                {
                    var mailmsg = new MailMessage
                    {
                        IsBodyHtml = true,
                        From = new MailAddress(from)
                    };
                    foreach (var addr in to)
                    {
                        mailmsg.To.Add(addr);
                    }
                    mailmsg.Subject = subject;
                    mailmsg.Body = body;
                    client.Send(mailmsg);
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " " + e.InnerException?.Message);
            }
        }
    }
}
