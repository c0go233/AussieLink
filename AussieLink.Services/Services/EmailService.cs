using AussieLink.Contracts.Services;
using AussieLink.Services.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Services
{
    public class EmailService : IEmailService
    {
        private readonly string SenderEmail = "c0go233@gmail.com";
        private readonly string SenderPassword = "rltjd123";

        public void SendResetPasswordLink(string userEmail, int linkCode, string returnUrl)
        {
            string encryptedLinkCode = Encryptor.GetEncryptedString(linkCode.ToString());
            string encryptedEmail = Encryptor.GetEncryptedString(userEmail);

            string emailBody = GetResetPasswordEmailBody(encryptedEmail, encryptedLinkCode, returnUrl);
            MailMessage email = GetMailMessage(emailBody, "AussieLink: Reset your password", userEmail);
            SendEmail(email);
        }

        private string GetResetPasswordEmailBody(string userEmail, string linkCode, string returnUrl)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<a href=" + returnUrl
                + "?email=" + userEmail
                + "&linkCode=" + linkCode
                + ">Please click this link to reset your password");

            return sb.ToString();
        }

        private MailMessage GetMailMessage(string emailBody, string title, string recieverEmail)
        {
            MailMessage mail = new MailMessage(SenderEmail, recieverEmail,
                title, emailBody);

            mail.IsBodyHtml = true;
            return mail;
        }

        private void SendEmail(MailMessage email)
        {
            NetworkCredential credential = new NetworkCredential(SenderEmail, SenderPassword);
            SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587);
            mailClient.EnableSsl = true;
            mailClient.Credentials = credential;
            mailClient.Send(email);
        }
    }
}
