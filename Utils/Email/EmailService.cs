using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace cinema_core.Utils.Email
{
    public static class EmailService
    {
        public static void SendEmail(Bitmap image,User user,Movie movie)
        {
            var fromAddress = new MailAddress("adm.cinex@gmail.com", "Admin Cinex");
            var toAddress = new MailAddress(user.Email, user.FullName);
            const string fromPassword = "admin@123456";
            string subject = "Ticket Movie : "+movie.Title;
            string body = "Dear "+user.FullName+" . Please save this image for checking in";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
            };

            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            stream.Position = 0;
            message.Attachments.Add(new Attachment(stream,"image.png"));
            {
                smtp.Send(message);
            }
        }
    }
}
