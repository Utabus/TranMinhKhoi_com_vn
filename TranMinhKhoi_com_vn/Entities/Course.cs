using System;
using System.Collections.Generic;

namespace TranMinhKhoi_com_vn.Entities;

public partial class Course
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public DateTime? Cdt { get; set; }

    public string? Link { get; set; }

    public string? Content { get; set; }

    public int? Donat { get; set; }

    public virtual ICollection<RequestCourse> RequestCourses { get; set; } = new List<RequestCourse>();
}
