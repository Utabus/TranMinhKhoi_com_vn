using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranMinhKhoi_com_vn.Entities;

public partial class Account
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Avartar { get; set; }

    public string? Email { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public int? Coin { get; set; }

    public DateTime? Birthday { get; set; }

    public byte? Gender { get; set; }

    public int? RoleId { get; set; }

    public byte? Status { get; set; }

    public string? Major { get; set; }
    [NotMapped]
    public string? ResetToken { get; set; }

    [NotMapped]
    public DateTime? ResetTokenExpiry { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Fund> Funds { get; set; } = new List<Fund>();

    public virtual ICollection<RequestCourse> RequestCourses { get; set; } = new List<RequestCourse>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<VipAccount> VipAccounts { get; set; } = new List<VipAccount>();
}
