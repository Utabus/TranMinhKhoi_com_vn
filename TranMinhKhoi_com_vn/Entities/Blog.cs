using System;
using System.Collections.Generic;

namespace TranMinhKhoi_com_vn.Entities;

public partial class Blog
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public int? AccountId { get; set; }

    public string? Content { get; set; }

    public string? Type { get; set; }

    public byte? Status { get; set; }

    public DateTime? Cdt { get; set; } = DateTime.Now;

    public virtual Account? Account { get; set; }
}
