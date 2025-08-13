using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;

namespace TranMinhKhoi_com_vn.Services
{
    public static class EmailService
    {
        public static async Task SendCourseEmailAsync(string toEmail, string userName, DateTime? buyDate)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("TranMinhKhoi.com.vn", "admin@example.com"));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Thông tin mua khóa học của bạn";

            email.Body = new TextPart("plain")
            {
                Text =
                    $"Xin chào {userName},\n\n" +
                    $"Cảm ơn bạn đã mua khóa học trên trang web của chúng tôi.\n" +
                    $"Thông tin khóa học:\n" +
                    $"Tên khóa học: Toàn bộ khóa học\n" +
                    $"Ngày mua: {buyDate?.ToString("dd/MM/yyyy")}\n" +
                    $"Trân trọng\n" +
                    $"TranMinhKhoi.com.vn"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("khangchannel19@gmail.com", "jnal cnyl mlit izco");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("TranMinhKhoi.com.vn", "admin@example.com"));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            email.Body = new TextPart("html")
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("khangchannel19@gmail.com", "jnal cnyl mlit izco");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}