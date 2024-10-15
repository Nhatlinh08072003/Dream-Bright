using System;
using System.Collections.Generic;

namespace Dream_Bridge.Models.Main;

public partial class Comment
{
    public int IdComment { get; set; }

    public int IdUser { get; set; }

    public int? IdNews { get; set; }

    public int? IdVisaService { get; set; }

    public string CommentText { get; set; } = null!;

    public int? Rating { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual News? IdNewsNavigation { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;
}
