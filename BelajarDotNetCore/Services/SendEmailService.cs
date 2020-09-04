using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BelajarDotNetCoreAPI.Services
{
    public class SendEmailService
    {
        public void SendEmail(string email, string emailRandomCode)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(email));
                message.From = new MailAddress("xyztechnologyid@gmail.com", "NetCore API Admin");
                message.Subject = "Registration at " + DateTime.Now.ToString("dddd, MMMM dd yyyy");
                message.Body = "Please don't share this code to anyone,<br>Registration Code :<br><br><b>" + emailRandomCode + "</b><br><br> Please enter this code when you want to verfy you account !";
                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.UseDefaultCredentials = false;
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("xyztechnologyid@gmail.com", "HelloWorld");
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }
    }
}
