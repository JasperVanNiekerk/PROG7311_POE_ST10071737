using EASendMail;

namespace Prog7311_POE_ST10071737.Services
{
    public class EmailService
    {
        private string senderEmail = "agrienergyconnect6969@gmail.com";
        private string senderPassword = "iaaahveqhebbcawz";

        public void Sender(string email, string name, string password)
        {
            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");

                // Your gmail email address
                oMail.From = senderEmail;
                // Set recipient email address
                oMail.To = email;

                // Set email subject
                oMail.Subject = "Email verivication";
                // Set email body
                oMail.TextBody = "Dear " + name + "\r\n" +
                    "your request to join the Agri-Energy Connect platform has been aprroved\r\n" +
                    "You can now Login onto the platform with the following credentials" +
                    "Email: " + email + "\r\n" +
                    "Password: " + password + "\r\n" +
                    "sincerely the Agri-Energy connect Team";

                // Gmail SMTP server address
                SmtpServer oServer = new SmtpServer("smtp.gmail.com");
                // Gmail user authentication
                // For example: your email is "gmailid@gmail.com", then the user should be the same
                oServer.User = senderEmail;

                // Create app password in Google account
                // https://support.google.com/accounts/answer/185833?hl=en
                oServer.Password = senderPassword;

                // Set 587 port, if you want to use 25 port, please change 587 5o 25
                oServer.Port = 465;

                // detect SSL/TLS automatically
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                //Console.WriteLine("start to send email over SSL ...");

                SmtpClient oSmtp = new SmtpClient();
                oSmtp.SendMail(oServer, oMail);

                //Console.WriteLine("email was sent successfully!");
            }
            catch (Exception ep)
            {
                //Console.WriteLine("failed to send email with the following error:");
                //Console.WriteLine(ep.Message);
            }
        }

    }
}
