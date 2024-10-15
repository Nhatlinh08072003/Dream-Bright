using System;
using System.Collections.Generic;

namespace Dream_Bridge.Models.Main;

public partial class News
{
    public int IdNews { get; set; }

    public string TitleNews { get; set; } = null!;

    public string NewsDescription { get; set; } = null!;

    public string NewsContent { get; set; } = null!;

    public string? NewsImage { get; set; }

    public int IdUser { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual User IdUserNavigation { get; set; } = null!;
}
