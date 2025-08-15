using AspNetCoreHero.ToastNotification.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MimeKit;
using TranMinhKhoi_com_vn.Areas.Admin.Controllers;
using TranMinhKhoi_com_vn.Entities;
using TranMinhKhoi_com_vn.Models;
using TranMinhKhoi_com_vn.Services; // Thêm dòng này ở đầu file

namespace TranMinhKhoi_com_vn.Controllers
{
    public class StudentLifeController : BaseController
    {
        public StudentLifeController(TranMinhKhoiDbContext context, INotyfService notyfService, IConfiguration configuration) : base(context, notyfService, configuration)
        {
        }

        public IActionResult Index()
        {
            var userName = User.Claims.SingleOrDefault(c => c.Type == "UserName");
            if (userName == null)
                return View();

            var user = _context.VipAccounts.Include(x => x.Account).FirstOrDefault(x => x.Account.UserName == userName.Value);
            return View(user);
        }
        public IActionResult Ielts()
        {
            return View();
        }
        public async Task<IActionResult> BuyCourse()
        {
            var userName = User.Claims.SingleOrDefault(c => c.Type == "UserName");
            var user = _context.Accounts.FirstOrDefault(x => x.UserName == userName.Value);
            if (user == null)
            {
                _notyfService.Error("Bạn chưa đăng nhập");
                return RedirectToAction("Login", "Account");
            }
            var data = new BuyCourse()
            {
                Account = user,
                KeySePay = _context.KeySePays.SingleOrDefault()
            };
            return View(data);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> BuyCourseHandle()
        {
            var userName = User.Claims.SingleOrDefault(c => c.Type == "UserName");
            var user = _context.Accounts.FirstOrDefault(x => x.UserName == userName.Value);
            if (user == null)
            {
                _notyfService.Error("Bạn chưa đăng nhập");
                return RedirectToAction("Login", "Account");
            }
            if (user.Coin == null || user.Coin < 200000)
            {
                _notyfService.Error("Số dư không đủ để mua khóa học");
                return RedirectToAction("BuyCourse");
            }
            if (_context.VipAccounts.Any(x => x.AccountId == user.Id))
            {
                _notyfService.Warning("Bạn đã sở hữu khóa học này!");
                return RedirectToAction("Index", "StudentLife");
            }

            user.Coin -= 200000;
            var vipAccount = new VipAccount
            {
                AccountId = user.Id,
                Cdt = DateTime.Now,
            };
            var history = new Fund
            {
                AccountId = user.Id,
                Total = 200000,
                InOut = false,
                Cdt = DateTime.UtcNow.AddHours(7),
                Content = null,
                Status = "Mua khóa học",
            };
            _context.Funds.Add(history);
            _context.VipAccounts.Add(vipAccount);
            await _context.SaveChangesAsync();

            // Gửi email cho user
            string subject = "[Tranminhkhoi.com.vn] Thông tin mua khóa học của bạn";
            string body = $@"
<html>
<head>
  <meta charset='UTF-8'>
  <style>
    body {{
        font-family: Arial, sans-serif;
        line-height: 1.6;
        color: #333;
    }}
    .container {{
        max-width: 600px;
        margin: auto;
        padding: 20px;
        border: 1px solid #ddd;
        border-radius: 10px;
        background-color: #f9f9f9;
    }}
    h2 {{
        color: #2c3e50;
    }}
    .info {{
        margin: 10px 0;
        padding: 10px;
        background: #fff;
        border-radius: 5px;
        border: 1px solid #eee;
    }}
    .footer {{
        margin-top: 20px;
        font-size: 14px;
        color: #777;
    }}
  </style>
</head>
<body>
  <div class='container'>
    <h2>Xin chào {user.UserName},</h2>
    <p>Cảm ơn bạn đã mua khóa học trên trang web của chúng tôi. Dưới đây là thông tin chi tiết:</p>
    
    <div class='info'>
      <p><strong>Tên:</strong> {user.FullName}</p>
      <p><strong>Email:</strong> {user.Email}</p>
      <p><strong>Chuyên ngành:</strong> {user.Major}</p>
    </div>

    <h3>Thông tin khóa học</h3>
    <div class='info'>
      <p><strong>Tên khóa học:</strong> Toàn bộ khóa học</p>
      <p><strong>Ngày mua:</strong> {vipAccount.Cdt?.ToString("dd/MM/yyyy")}</p>
    </div>

    <p>Chúc bạn học tập hiệu quả và thành công!</p>
    <div class='footer'>
      Trân trọng,<br>
      <strong>TranMinhKhoi.com.vn</strong>
    </div>
  </div>
</body>
</html>
";
            try
            {
                await EmailService.SendEmailAsync(user.Email, subject, body);
                var adminEmail = _context.KeySePays.FirstOrDefault();
                if (adminEmail != null)
                {
                    string adminSubject = $"[Tranminhkhoi.com.vn] {user.Email} đã mua khóa học";

                    string adminBody = $@"
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
        }}
        .container {{
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 8px;
            background-color: #f9f9f9;
            max-width: 500px;
        }}
        h2 {{
            color: #2c3e50;
            margin-bottom: 10px;
        }}
        p {{
            margin: 8px 0;
        }}
        .highlight {{
            color: #007BFF;
            font-weight: bold;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h2>Thông báo mua khóa học</h2>
        <p><strong>Người dùng:</strong> <span class='highlight'>{user.UserName}</span></p>
        <p><strong>Email:</strong> {user.Email}</p>
        <p><strong>Chuyên ngành:</strong> {user.Major}</p>
        <p><strong>Ngày mua:</strong> {vipAccount.Cdt?.ToString("dd/MM/yyyy")}</p>
    </div>
</body>
</html>
";

                    await EmailService.SendEmailAsync(adminEmail.Email, adminSubject, adminBody);
                }
            }
            catch
            {
                _notyfService.Warning("Gửi email thất bại!");
            }

            _notyfService.Success("Mua thành công khóa học");
            return RedirectToAction("Index", "StudentLife");
        }

        public async Task<IActionResult> StudentLifeDetail(string Type)
        {
            return View(await _context.Blogs.FirstOrDefaultAsync(c => c.Type == Type.Trim()));
        }

        public async Task<IActionResult> CoursesDetail(int id)
        {
            try
            {
                return View(await _context.Courses.FirstOrDefaultAsync(c => c.Id == id));
                //var userName = User.Claims.SingleOrDefault(c => c.Type == "UserName");
                //var email = User.Claims.SingleOrDefault(c => c.Type == "Email");
                //var requestCourse = await _context.RequestCourses
                //    .Include(r => r.Account)
                //    .Include(r => r.Course)
                //    .FirstOrDefaultAsync(m => m.CourseId == id && m.Account.UserName == userName.Value);
                //if (requestCourse != null)
                //{
                //    if (requestCourse.Status == true)
                //    {
                //        return View(await _context.Courses.FirstOrDefaultAsync(c => c.Id == id));
                //    }
                //    else
                //    {
                //        _notyfService.Warning("Bạn đã gửi yêu cầu đăng ký khóa học này, vui lòng chờ duyệt!");
                //        return RedirectToAction("WaitForApprove");
                //    }
                //}
                //else
                //{
                //    var adminEmail = _context.KeySePays.FirstOrDefault();
                //    var account = _context.Accounts.FirstOrDefault(x => x.UserName == userName.Value);
                //    if (adminEmail != null)
                //    {
                //        string adminSubject = $"{account?.Email} yêu cầu xem khóa học";
                //        string adminBody = $"User: {userName?.Value} yêu cầu tham gia khóa học vui lòng xác nhận";
                //        await EmailService.SendEmailAsync(adminEmail.Email, adminSubject, adminBody);
                //    }
                //    var request = new RequestCourse
                //    {
                //        AccountId = account.Id,
                //        CourseId = id,
                //        Cdt = DateTime.UtcNow.AddHours(7),
                //        Status = false
                //    };
                //    _context.RequestCourses.Add(request);
                //    await _context.SaveChangesAsync();

                //    _notyfService.Warning("Bạn đã gửi yêu cầu đăng ký khóa học này, vui lòng chờ duyệt!");
                //    return RedirectToAction("WaitForApprove");

                //}
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IActionResult> Courses()
        {
            var user = User.Identity;
            if (user == null || !user.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(await _context.Courses.ToListAsync());
        }

        public IActionResult WaitForApprove()
        {
            return View();
        }


        public async Task<IActionResult> IeltsBlog()
        {
            return View(await _context.Blogs.Where(c => c.Type == "Ielts").OrderByDescending(x => x.Cdt).ToListAsync());
        }



        public async Task<IActionResult> SoftSkill()
        {
            return View(await _context.Blogs.Where(c => c.Type == "SoftSkill").OrderByDescending(x => x.Cdt).ToListAsync());
        }


    }
}
