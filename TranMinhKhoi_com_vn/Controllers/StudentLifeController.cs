using AspNetCoreHero.ToastNotification.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using TranMinhKhoi_com_vn.Areas.Admin.Controllers;
using TranMinhKhoi_com_vn.Entities;
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

            return View(user);  
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
            string subject = "Thông tin mua khóa học của bạn";
            string body =
                $"Xin chào {user.UserName},\n\n" +
                $"Cảm ơn bạn đã mua khóa học trên trang web của chúng tôi.\n" +
                $"Thông tin khóa học:\n" +
                $"Tên khóa học: Toàn bộ khóa học\n" +
                $"Ngày mua: {vipAccount.Cdt?.ToString("dd/MM/yyyy")}\n" +
                $"Trân trọng\n" +
                $"TranMinhKhoi.com.vn";
            try
            {
                await EmailService.SendEmailAsync(user.Email, subject, body);
                var adminEmail = _context.KeySePays.FirstOrDefault();
                if (adminEmail != null)
                {
                    string adminSubject = $"{user.Email} đã mua khóa học";
                    string adminBody = $"User: {user.UserName} đã mua khóa học lúc {vipAccount.Cdt?.ToString("dd/MM/yyyy")}";
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
            var user = User.Identity;
            if (user == null || !user.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(await _context.Courses.FirstOrDefaultAsync(c => c.Id == id));
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
    }
}
