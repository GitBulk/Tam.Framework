using System;
using System.Net.Mail;

namespace Tam.Util
{
    public static class MailHelper
    {
        public static bool SendMailRegister(string userName, string userToken,
            string emailToSend, bool useGmail = true)
        {
            try
            {
                // http://www.codeproject.com/Tips/520998/Send-Email-from-Yahoo-GMail-Hotmail-Csharp
                var mail = new MailMessage();
                string emailFrom = "";
                string subject = "";
                string body = "";
                string smtpAdddress = "smtp.gmail.com";

                // password smtp
                string password = "";
                mail.To.Add(emailToSend);

                mail.From = new MailAddress(emailFrom);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtpAdddress;
                smtp.Port = 587;
                if (useGmail == false)
                {
                    smtp.Host = "";
                }

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(emailFrom, password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}