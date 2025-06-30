using System;
using System.Collections.Generic;

namespace TranMinhKhoi_com_vn.Entities;

public partial class Role
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
