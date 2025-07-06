using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TranMinhKhoi_com_vn.Entities;
using TranMinhKhoi_com_vn.Extension;
using TranMinhKhoi_com_vn.Helper;

namespace TranMinhKhoi_com_vn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : BaseController
    {
        public AccountsController(TranMinhKhoiDbContext context, INotyfService notyfService, IConfiguration configuration) : base(context, notyfService, configuration)
        {
        }

        // GET: Admin/Accounts
        public async Task<IActionResult> Index()
        {
            var tranMinhKhoiDbContext = _context.Accounts.Include(a => a.Role);
            return View(await tranMinhKhoiDbContext.ToListAsync());
        }

        // GET: Admin/Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Admin/Accounts/Create
        public IActionResult Create()
        {
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account account, IFormFile fAvatar)
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
            var mk = "123123";
            var mailex = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == account.Email || x.UserName == account.UserName);
            if (mailex != null)
            {
                _notyfService.Error("Email hay tài khoản đã tồn tại");
                return View(account);
            }
            if (fAvatar != null)
            {
                string extennsion = Path.GetExtension(fAvatar.FileName);
                image = Utilities.ToUrlFriendly(account.UserName ?? "") + extennsion;
                account.Avartar = await Utilities.UploadFile(fAvatar, @"User", image.ToLower());
            }
            account.Avartar = "UserDemo.jpg";
            account.Password = mk.ToMD5();
            account.Coin = 0;
            account.RoleId = 2;
            account.FullName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(account.FullName ?? "");
            _context.Add(account);
            await _context.SaveChangesAsync();
            _notyfService.Success("Thêm thành công");
            return RedirectToAction("Index");
        }

        // GET: Admin/Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Account account, IFormFile fAvatar)
        {
            try
            {
                var nhanvien = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id);
                if (nhanvien == null)
                {
                    return NotFound();
                }
                var ktemail = await _context.Accounts.FirstOrDefaultAsync(x => x.Id != account.Id && (x.Email == account.Email && x.UserName == account.UserName));
                if (ktemail != null)
                {
                    _notyfService.Error("Email hay tên đăng nhập đã tồn tại trong hệ thống!");
                    return View(account);
                }
                if (fAvatar != null)
                {
                    string extennsion = Path.GetExtension(fAvatar.FileName);
                    image = Utilities.ToUrlFriendly(account.UserName ?? "") + extennsion;
                    nhanvien.Avartar = await Utilities.UploadFile(fAvatar, @"User", image.ToLower());
                }
                else
                {
                    account.Avartar = _context.Accounts.Where(x => x.Id == account.Id).Select(x => x.Avartar).FirstOrDefault();
                }
                nhanvien.FullName = account.FullName;
                nhanvien.Email = account.Email;
                nhanvien.Birthday = account.Birthday;
                nhanvien.Gender = account.Gender;
                nhanvien.Status = account.Status;
                nhanvien.UserName = account.UserName;
                
                _notyfService.Success("Sửa thành công!");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(account.Id))
                {
                    _notyfService.Error("Lỗi!!!!!!!!!!!!");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Index");
        }

        // GET: Admin/Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Admin/Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}
