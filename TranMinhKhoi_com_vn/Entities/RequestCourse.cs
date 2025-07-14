using System;
using System.Collections.Generic;

namespace TranMinhKhoi_com_vn.Entities;

public partial class RequestCourse
{
    public int Id { get; set; }

    public int? AccountId { get; set; }

    public int? CourseId { get; set; }

    public DateTime? Cdt { get; set; }
    public bool? Status { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Course? Course { get; set; }
}
