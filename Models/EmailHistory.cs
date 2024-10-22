public class EmailHistory
{
    public int Id { get; set; } // Khóa chính
    public string? ToEmail { get; set; } // Địa chỉ email người nhận
    public string? Subject { get; set; } // Tiêu đề email
    public string? Body { get; set; } // Nội dung email
    public DateTime SentAt { get; set; } // Thời điểm gửi email
    public bool IsSuccessful { get; set; } // Trạng thái gửi email (thành công hoặc thất bại)
    public string? ErrorMessage { get; set; } // Thông báo lỗi nếu việc gửi thất bại (nếu có)
}
