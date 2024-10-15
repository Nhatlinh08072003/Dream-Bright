using System;
using System.Collections.Generic;

namespace Dream_Bridge.Models.Main;

public partial class User
{
    public int IdUser { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool IsConsultant { get; set; }

    public string ConsultingStatus { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ChatMessage> ChatMessageReceivers { get; set; } = new List<ChatMessage>();

    public virtual ICollection<ChatMessage> ChatMessageSenders { get; set; } = new List<ChatMessage>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<StudyAbroadCatalog> StudyAbroadCatalogs { get; set; } = new List<StudyAbroadCatalog>();
}
