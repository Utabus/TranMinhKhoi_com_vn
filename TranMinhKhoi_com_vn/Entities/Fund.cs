using System;
using System.Collections.Generic;

namespace TranMinhKhoi_com_vn.Entities;

public partial class Fund
{
    public int Id { get; set; }

    public decimal? Total { get; set; }

    public bool? InOut { get; set; }

    public DateTime? Cdt { get; set; }

    public string? Status { get; set; }

    public string? Content { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }
}
