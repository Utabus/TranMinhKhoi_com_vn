using System;
using System.Collections.Generic;

namespace TranMinhKhoi_com_vn.Entities;

public partial class KeySePay
{
    public int Id { get; set; }

    public string? KeyApi { get; set; }

    public string? NumberBank { get; set; }

    public string? Name { get; set; }

    public DateTime? Cdt { get; set; }

    public bool? Status { get; set; }

    public string? Email { get; set; }
    public int? Coin { get; set; }
}
