using System;
using System.Collections.Generic;

namespace Dream_Bridge.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<School> Schools { get; set; } = new List<School>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
