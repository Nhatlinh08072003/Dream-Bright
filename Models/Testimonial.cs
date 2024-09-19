using System;
using System.Collections.Generic;

namespace Dream_Bridge.Models;

public partial class Testimonial
{
    public int TestimonialId { get; set; }

    public int? StudentId { get; set; }

    public string Content { get; set; } = null!;

    public int? Rating { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Student? Student { get; set; }
}
