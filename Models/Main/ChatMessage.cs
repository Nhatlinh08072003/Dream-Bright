using System;
using System.Collections.Generic;

namespace Dream_Bridge.Models.Main;

public class ChatMessage  
{  
    public int IdMessage { get; set; }  
    public int SenderId { get; set; }  
    public int ReceiverId { get; set; }  
    public string MessageText { get; set; } = null!;  
    public DateTime? CreatedAt { get; set; }  
    public virtual User Receiver { get; set; } = null!;  
    public virtual User Sender { get; set; } = null!;  
    public string? AttachmentUrl { get; set; } // Thêm thuộc tính tệp đính kèm  

    // Thêm các thuộc tính mới  
    public string SenderName => Sender?.FullName; // Giả sử User có thuộc tính FullName  
    public string Content => MessageText;  
    public DateTime TimeStamp => CreatedAt ?? DateTime.Now; // Sử dụng thời gian hiện tại nếu CreatedAt là null  
}
