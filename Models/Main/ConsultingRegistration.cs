using System;
using System.Collections.Generic;

namespace Dream_Bridge.Models.Main;

public partial class ConsultingRegistration
{
    public int IdConsulting { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string ConsultingType { get; set; } = null!;

    public string ContentConsulting { get; set; } = null!;

    public string? Note { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
