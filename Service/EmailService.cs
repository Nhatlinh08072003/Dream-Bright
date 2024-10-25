using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Dream_Bridge.Models.Main; // Đảm bảo bạn có namespace cho EmailHistory

public class EmailService
{
    private readonly StudyAbroadDbContext _dbContext;

    public EmailService(StudyAbroadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("DREAM-BRIGHT TƯ VẤN DU HỌC ", "letiennhatlinh08072003@gmail.com"));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("letiennhatlinh08072003@gmail.com", "fyjwdatfhtfinfmm");
                await client.SendAsync(message);

                // Lưu thông tin vào cơ sở dữ liệu sau khi gửi thành công
                var emailHistory = new EmailHistory
                {
                    ToEmail = toEmail,
                    Subject = subject,
                    Body = body,
                    SentAt = DateTime.UtcNow,
                    IsSuccessful = true, // Gửi thành công
                    ErrorMessage = null // Không có lỗi
                };

                _dbContext.EmailHistories.Add(emailHistory);
                _dbContext.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

                return true; // Trả về true nếu gửi và lưu thành công
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và ghi nhật ký
                var emailHistory = new EmailHistory
                {
                    ToEmail = toEmail,
                    Subject = subject,
                    Body = body,
                    SentAt = DateTime.UtcNow,
                    IsSuccessful = false, // Không gửi thành công
                    ErrorMessage = ex.Message // Ghi lại thông báo lỗi
                };

                _dbContext.EmailHistories.Add(emailHistory);
                _dbContext.SaveChanges(); // Lưu thông tin lỗi vào cơ sở dữ liệu

                return false; // Trả về false nếu có lỗi xảy ra
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
