using System;
using System.Collections.Generic;

namespace Dream_Bridge.Models.Main;

public partial class SchoolVSCatalog
{
    public int IdSchool { get; set; }

    public string ImageSchool { get; set; } = null!;

    public string TitleSchool { get; set; } = null!;

    public string SchoolDescription { get; set; } = null!;

    public string Nation { get; set; } = null!;

    public string StateCity { get; set; } = null!;

    public decimal AverageTuition { get; set; }

    public string Level { get; set; } = null!;

    public virtual StudyAbroadCatalog IdcategoryStabNavigation { get; set; } = null!;
    public int IdcategoryStab { get; set; }

    public string NamecategoryStab { get; set; } = null!;

    public int IdUser { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<School> Schools { get; set; } = new List<School>();
}
