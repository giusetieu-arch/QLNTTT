using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace QLNTTT
{
    public class GuiMail
    {
        public static string SendMail(
            string mailNhan,
            string tenNguoiNhan,
            string maHopDong,
            string filePDF)
        {
            try
            {
                // =========================
                // MAIL NGƯỜI GỬI
                // =========================

                string fromMail =
                    "chutro.manager@gmail.com";

                string password =
                    "qscf dxsa stqz hqdo";

                // =========================
                // SMTP
                // =========================

                SmtpClient smtp =
                    new SmtpClient("smtp.gmail.com");

                smtp.Port = 587;

                smtp.Credentials =
                    new NetworkCredential(
                        fromMail,
                        password);

                smtp.EnableSsl = true;

                // =========================
                // MAIL
                // =========================

                MailMessage mail =
                    new MailMessage();

                mail.From =
                    new MailAddress(fromMail);

                mail.To.Add(mailNhan);

                mail.Subject =
                    "Hợp đồng thuê phòng - " +
                    maHopDong;

                mail.SubjectEncoding =
                    Encoding.UTF8;

                mail.BodyEncoding =
                    Encoding.UTF8;

                mail.IsBodyHtml = true;

                // =========================
                // BODY
                // =========================

                mail.Body =
                    "<h2>HỢP ĐỒNG THUÊ PHÒNG</h2>" +
                    "<p>Xin chào <b>" +
                    tenNguoiNhan +
                    "</b></p>" +

                    "<p>Đây là hợp đồng thuê phòng của bạn.</p>" +

                    "<p>Mã hợp đồng: <b>" +
                    maHopDong +
                    "</b></p>" +

                    "<p>Vui lòng kiểm tra file đính kèm.</p>" +

                    "<br>" +

                    "<p>QLNTTT System</p>";

                // =========================
                // FILE PDF
                // =========================

                if (File.Exists(filePDF))
                {
                    Attachment file =
                        new Attachment(filePDF);

                    mail.Attachments.Add(file);
                }

                // =========================
                // SEND
                // =========================

                smtp.Send(mail);

                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}