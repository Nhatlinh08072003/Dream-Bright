using System;
using System.Collections.Generic;

namespace Dream_Bridge.Models;

public partial class Consultation
{
    public int ConsultationId { get; set; }

    public int? StudentId { get; set; }

    public int? EmployeeId { get; set; }

    public DateOnly? ConsultationDate { get; set; }

    public string? ConsultationType { get; set; }

    public string? Notes { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Student? Student { get; set; }
}
