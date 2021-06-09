using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AssetManagementAPI.Handler
{
    public class SendMail
    {
        public void SendEmail(string emailAddress, string body, string subject)
        {
            MailMessage mailMessage = new MailMessage("helpdesk.assetmanagement.mcc49@gmail.com", emailAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            NetworkCredential networkCredential = new NetworkCredential("helpdesk.assetmanagement.mcc49@gmail.com", "SaipulAripin123");
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.Send(mailMessage);
        }
    }
}
