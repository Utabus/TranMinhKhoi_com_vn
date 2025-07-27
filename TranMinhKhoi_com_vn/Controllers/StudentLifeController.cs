using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TranMinhKhoi_com_vn.Areas.Admin.Controllers;
using TranMinhKhoi_com_vn.Entities;

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
            user.Coin = user.Coin - 200000; 
            var vipAccount = new VipAccount
            {
                AccountId = user?.Id,
                Cdt = DateTime.Now,
            };
            _context.VipAccounts.Add(vipAccount);
            await _context.SaveChangesAsync();
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

       
    }
}
