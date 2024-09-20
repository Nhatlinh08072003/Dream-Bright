using System;
using System.Collections.Generic;

namespace Dream_Bridge.Models;

public partial class School
{
    public int SchoolId { get; set; }

    public string SchoolName { get; set; } = null!;

    public int? CountryId { get; set; }

    public string? Description { get; set; }

    public string? Address { get; set; }

    public string? Website { get; set; }

    public virtual Country? Country { get; set; }
}
