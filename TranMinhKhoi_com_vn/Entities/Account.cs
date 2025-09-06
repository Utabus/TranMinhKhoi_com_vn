using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranMinhKhoi_com_vn.Entities;

public partial class Account
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3-50 ký tự")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6-100 ký tự")]
    public string? Password { get; set; }

    [StringLength(500, ErrorMessage = "Đường dẫn avatar không được vượt quá 500 ký tự")]
    public string? Avartar { get; set; }

    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Họ tên là bắt buộc")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Họ tên phải từ 2-100 ký tự")]
    [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "Họ tên chỉ được chứa chữ cái và khoảng trắng")]
    public string? FullName { get; set; }

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [StringLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự")]
    [RegularExpression(@"^[0-9+\-\s()]+$", ErrorMessage = "Số điện thoại chỉ được chứa số và các ký tự +, -, (, ), khoảng trắng")]
    public string? Phone { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Số coin không được âm")]
    public decimal? Coin { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? Birthday { get; set; }

    [Range(0, 2, ErrorMessage = "Giới tính phải là 0 (Không xác định), 1 (Nam) hoặc 2 (Nữ)")]
    public byte? Gender { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "RoleId phải lớn hơn 0")]
    public int? RoleId { get; set; }

    [Range(0, 2, ErrorMessage = "Trạng thái phải là 0 (Không hoạt động), 1 (Hoạt động) hoặc 2 (Tạm khóa)")]
    public byte? Status { get; set; }

    [StringLength(100, ErrorMessage = "Chuyên ngành không được vượt quá 100 ký tự")]
    public string? Major { get; set; }

    [NotMapped]
    [StringLength(100, ErrorMessage = "Reset token không được vượt quá 100 ký tự")]
    public string? ResetToken { get; set; }

    [NotMapped]
    [DataType(DataType.DateTime)]
    public DateTime? ResetTokenExpiry { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Fund> Funds { get; set; } = new List<Fund>();

    public virtual ICollection<RequestCourse> RequestCourses { get; set; } = new List<RequestCourse>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<VipAccount> VipAccounts { get; set; } = new List<VipAccount>();
}
