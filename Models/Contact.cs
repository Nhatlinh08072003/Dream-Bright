using System;
using System.Collections.Generic;

namespace Dream_Bridge.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public int? StudentId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime? SubmittedAt { get; set; }

    public virtual Student? Student { get; set; }
}
