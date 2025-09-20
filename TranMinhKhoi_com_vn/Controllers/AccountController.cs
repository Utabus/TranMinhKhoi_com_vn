using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TranMinhKhoi_com_vn.Entities;
using TranMinhKhoi_com_vn.Extension;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using TranMinhKhoi_com_vn.Models;
using System.ComponentModel.DataAnnotations;
using TranMinhKhoi_com_vn.Areas.Admin.Controllers;

namespace TranMinhKhoi_com_vn.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(TranMinhKhoiDbContext context, INotyfService notyfService, IConfiguration configuration) : base(context, notyfService, configuration)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string user, string pass)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var password = pass.ToMD5();
            var account = await _context.Accounts.FirstOrDefaultAsync(u => u.UserName == user && u.Password == password);

            if (account == null)
            {
                // Tên đăng nhập hoặc mật khẩu không đúng
                _notyfService.Error("Thông tin đăng nhập không chính xác");
                return RedirectToAction("Login", "Account");
            }
            if (account?.RoleId == 1 )
            {
                _notyfService.Error("Tài khoản của bạn là tài khoản Admin");
                return RedirectToAction("Login", "Account");
            }
            if (account?.Status == 2)
            {
                _notyfService.Error("Tài khoản đã bị khóa");
                return RedirectToAction("Login", "Account");
            }
            if (account != null)
            {

                var random = new Random();
                var token = random.Next(100000, 999999).ToString();
                account.ResetToken = token;
                account.ResetTokenExpiry = DateTime.UtcNow.AddHours(7).AddMinutes(10);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("OTP", token);
                HttpContext.Session.SetString("Email", account.Email ?? "");
                HttpContext.Session.SetString("UserName", account.UserName ?? "");
                HttpContext.Session.SetString("ResetTokenExpiry", account.ResetTokenExpiry.ToString() ?? "");
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("TranMinhKhoi.com.vn", "admin@example.com"));
                email.To.Add(MailboxAddress.Parse($"{account.Email}"));
                email.Subject = "[TranMinhKhoi.com.vn] Mã xác thực OTP";

                email.Body = new TextPart("plain")
                {
                    Text = $"Mã xác thực OTP của bạn là : {token} OTP có thời hạn là 10 phút\n" +
                    $"Vui lòng nhập mã OTP để đăng nhập vào hệ thống\n" +
                    $"Nếu bạn không yêu cầu đăng nhập, vui lòng bỏ qua email này\n" +
                    $"Trân trọng\n" +
                    $"TranMinhKhoi.com.vn"
                };
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("khangchannel19@gmail.com", "jnal cnyl mlit izco");
                smtp.Send(email);
                smtp.Disconnect(true);

                return RedirectToAction("LoginOTP", "Account");
            }
            else
            {
                _notyfService.Warning("Tên đăng nhập hoặc mật khẩu không đúng");
                return RedirectToAction("Login", "Account");
            }
        }
        public IActionResult Profile()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                var account = _context.Accounts.FirstOrDefault(u => u.Id == userId);
                if (account != null)
                    return View(account);
            }
            return RedirectToAction("Login", "Account");
        }
        public IActionResult LoginOTP()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginOTP(string otp)
        {
            var resetToken = HttpContext.Session.GetString("OTP");
            var resetTokenExpiry = HttpContext.Session.GetString("ResetTokenExpiry");
            var email = HttpContext.Session.GetString("Email");
            var userName = HttpContext.Session.GetString("UserName");

            if (!DateTime.TryParse(resetTokenExpiry, out var expiry) || expiry < DateTime.UtcNow)
            {
                _notyfService.Error("Token đã hết hạn. Vui lòng gửi lại yêu cầu đặt lại mật khẩu.");
                return View();
            }

            var user = await _context.Accounts.Include(x => x.Role).FirstOrDefaultAsync(u => u.Email == email && u.UserName == userName);
            if (user != null && resetToken == otp.Trim())
            {

                List<Claim> claims = new List<Claim>()
               {
                   new Claim(ClaimTypes.Name, user.FullName??""),
                   new Claim("UserName" , user.UserName ?? ""),
                new Claim(ClaimTypes.Role , user.Role?.Name ?? ""),
                   new Claim("Id" , user.Id.ToString()),
                    new Claim("Avartar", "/contents/Images/User/" + user.Avartar)
               };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                _notyfService.Success("Đăng nhập thành công");
                return RedirectToAction("Index", "Home");
            }

            _notyfService.Error("Token không hợp lệ .");

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _notyfService.Success("Đăng xuất thành công");
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string pass, string newpass, string confirmpass)
        {
            if (ModelState.IsValid)
            {
                var tendangnhapclam = User.Claims.SingleOrDefault(c => c.Type == "UserName");
                var tendangnhap = "";
                if (tendangnhapclam != null)
                { tendangnhap = tendangnhapclam.Value; }
                var password = pass.ToMD5();
                var user = await _context.Accounts.FirstOrDefaultAsync(u => u.UserName == tendangnhap);
                if (user?.Password != password)
                {
                    _notyfService.Error("Mật khẩu cũ không chính xác");
                    return RedirectToAction("Index", "Home");
                }
                if (newpass.Length < 6 && newpass.Length < 100)
                {
                    _notyfService.Error("Mật khẩu mới phải trên 6 ký tự và nhỏ hơn 100 ký tự ");
                    return RedirectToAction("Index", "Home");
                }
                if (newpass != confirmpass)
                {
                    _notyfService.Error("Mật khẩu mới không đúng với mật khẩu xác nhận !");
                    return RedirectToAction("Index", "Home");
                }
                user.Password = newpass.ToMD5();
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                _notyfService.Error("Vui lòng nhập đầy đủ thông mật khẩu !");

            }
            _notyfService.Success("Đổi mật khẩu thành công!");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Account account)
        {
            if (account.UserName?.Length < 6)
            {
                _notyfService.Error("Tài khoản không bé hơn 6 kí tự");
                return View(account);
            }
            if (account.Password?.Length < 6)
            {
                _notyfService.Error("Mật khẩu không bé hơn 6 kí tự");
                return View(account);
            }
            if (account.Phone?.Length != 10)
            {
                _notyfService.Error("Số điện thoại là 10 số");
                return View(account);
            }
            var exaccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == account.Email || x.UserName == account.UserName);
            if (exaccount != null)
            {
                _notyfService.Error("Email hoặc Username đã tồn tại");
                return View(account);
            }
            account.Avartar = "UserDemo.jpg";
            account.Password = (account.Password)?.ToMD5();
            account.Status = 1;
            account.Coin = 0; 
            account.RoleId = 2;

            _context.Update(account);
            await _context.SaveChangesAsync();
            _notyfService.Success("Đăng ký thành công");
            return RedirectToAction("Login", "Account");
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email) || !new EmailAddressAttribute().IsValid(Email))
            {
                _notyfService.Warning("Email không hợp lệ.");
                return View();
            }
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == Email);
            if (user != null)
            {
                var random = new Random();
                var token = random.Next(100000, 999999).ToString();
                user.ResetToken = token;
                user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
                user.Email = Email;
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("ResetToken", token);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetString("ResetTokenExpiry", user.ResetTokenExpiry.ToString() ?? "");
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("TranMinhKhoi.com.vn", "admin@example.com"));
                email.To.Add(MailboxAddress.Parse($"{Email}"));
                email.Subject = $"[TranMinhKhoi.com.vn] Yêu cầu đặt lại mật khẩu User :{user.UserName}";

                email.Body = new TextPart("plain")
                {
                    Text = $"Để đặt lại mật khẩu, User : {user.Email}" +
                    $"Vui lòng sử dụng token sau đây: {token} mã token có thời hạn là 10 phút"
                };
                try
                {
                    using var smtp = new SmtpClient();
                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate("khangchannel19@gmail.com", "jnal cnyl mlit izco");
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
                catch (Exception)
                {
                    _notyfService.Error("Không thể gửi email. Vui lòng thử lại sau.");
                    return View();
                }


                return RedirectToAction(nameof(ResetPassword));

            }
            else
            {
                _notyfService.Warning("Email không tồn tại trong hệ thống ");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost()]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var resetToken = HttpContext.Session.GetString("ResetToken");
            var resetTokenExpiry = HttpContext.Session.GetString("ResetTokenExpiry");
            var email = HttpContext.Session.GetString("Email");

            if (!DateTime.TryParse(resetTokenExpiry, out var expiry) || expiry < DateTime.UtcNow)
            {
                _notyfService.Error("Token đã hết hạn. Vui lòng gửi lại yêu cầu đặt lại mật khẩu.");
                return View(model);
            }

            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null && resetToken == model.Token)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    TempData["ResetPasswordErrorMessage"] = "Mật khẩu mới và xác nhận không khớp.";
                    return View(model);
                }

                user.Password = model.Password.ToMD5();
                user.ResetToken = null;
                user.ResetTokenExpiry = null;
                await _context.SaveChangesAsync();

                _notyfService.Success("Đặt lại mật khẩu thành công.");
                return RedirectToAction("Login", "Account");
            }

            _notyfService.Error("Token không hợp lệ hoặc email không tồn tại.");

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Policy()
        {
            return View(await _context.Blogs.FirstOrDefaultAsync(c => c.Type == "Privacy-Policy"));
        }

    }
}
