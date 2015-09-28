using System.Net;
using System.Net.Mail;

namespace GAP.Helpers
{
    public class EmailSender
    {

        public void ResetPassword(string ToEmail, string password)
        {
            var fromAddress = new MailAddress("gap.donotreply@gmail.com", "Datalog Technologies Inc - GAP");
            var toAddress = new MailAddress(ToEmail);
            const string fromPassword = "datalog.gap.2013";
            const string subject = "Datalog Technologies Inc - GAP Password Reset";
            string body = "Your new credentials for the Datalog Technologies Inc - GAP Dashboard \n" +
                "Username: " + ToEmail + "\n" +
                "Password: " + password + "\n";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        public void UserCreate(string ToEmail, string pass, string serial)
        {
            var fromAddress = new MailAddress("gap.donotreply@gmail.com", "Datalog Technologies Inc - GAP");
            var toAddress = new MailAddress(ToEmail);
            const string fromPassword = "datalog.gap.2013";
            const string subject = "Datalog Technologies Inc - GAP - New User";
            string body = "Your new credentials for the Datalog Technologies Inc - GAP \n" +
                "Username: " + ToEmail + "\n" +
                "Password: " + pass + "\n" +
                "Serial Number: " + serial + "\n";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        private void createHTMLMessage()
        {

        }
    }
}
