using System;
using System.Collections.Generic;

namespace Dream_Bridge.Models.Main;

public partial class ChatMessage
{
    public int IdMessage { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public string MessageText { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual User Receiver { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
