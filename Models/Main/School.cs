using System;
using System.Collections.Generic;

namespace Dream_Bridge.Models.Main;

public partial class School
{
    public int IdSchool { get; set; }

    public string ImageSchool { get; set; } = null!;

    public string TitleSchool { get; set; } = null!;

    public string SchoolDescription { get; set; } = null!;

    public string Nation { get; set; } = null!;

    public string StateCity { get; set; } = null!;

    public decimal AverageTuition { get; set; }

    public string Level { get; set; } = null!;

    public int IdcategoryStab { get; set; }
    public string DetailedDescription { get; set; }

    public virtual StudyAbroadCatalog IdcategoryStabNavigation { get; set; } = null!;
}
