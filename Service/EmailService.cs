    using MailKit.Net.Smtp;
    using MailKit.Security;
    using MimeKit;

    public class EmailService
    {
        public void SendEmail(string toEmail, string subject, string body)
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
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate("letiennhatlinh08072003@gmail.com", "fyjwdatfhtfinfmm"); // Sử dụng mật khẩu ứng dụng
                client.Send(message);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và ghi nhật ký
                throw new Exception("Could not send email: " + ex.Message);
            }
            finally
            {
                client.Disconnect(true);
            }
        }
    }

    }


