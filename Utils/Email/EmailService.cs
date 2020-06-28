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
        public static void SendEmail(Bitmap image)
        {
            var fromAddress = new MailAddress("adm.cinex@gmail.com", "Admin Cinex");
            var toAddress = new MailAddress("lhthang.98@gmail.com", "Thang Le");
            const string fromPassword = "admin@123456";
            const string subject = "Ticket Movie : ";
            const string body = "Please save this image for checking in";

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
