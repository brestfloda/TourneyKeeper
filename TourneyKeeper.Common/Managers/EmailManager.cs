using System;
using System.Linq;
using System.Net.Mail;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Common.Managers
{
    public class EmailManager
    {
        public void SendNewPassword(string email, string newPassword)
        {
            MailMessage message = new MailMessage();
            message.To.Add(email);
            message.Subject = "TourneyKeeper: new password";
            message.From = new MailAddress("admin@tourneykeeper.net");
            message.Body = "Your new password: " + newPassword;

            SmtpClient smtp = new SmtpClient("smtp.unoeuro.com");
            smtp.Send(message);
        }
    }
}
