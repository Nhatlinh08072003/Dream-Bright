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
}

