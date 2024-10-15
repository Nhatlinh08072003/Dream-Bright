using System;
using System.Collections.Generic;

namespace Dream_Bridge.Models.Main;

public partial class StudyAbroadCatalog
{
    public int IdcategoryStab { get; set; }

    public string NamecategoryStab { get; set; } = null!;

    public int IdUser { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<School> Schools { get; set; } = new List<School>();
}
