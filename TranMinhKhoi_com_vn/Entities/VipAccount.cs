using System;
using System.Collections.Generic;

namespace TranMinhKhoi_com_vn.Entities;

public partial class VipAccount
{
    public int Id { get; set; }

    public int? AccountId { get; set; }

    public DateTime? Cdt { get; set; }

    public virtual Account? Account { get; set; }
}
