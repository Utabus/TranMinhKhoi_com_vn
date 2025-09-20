using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TranMinhKhoi_com_vn.Entities;
using TranMinhKhoi_com_vn.Helper;

namespace TranMinhKhoi_com_vn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogsController : BaseController
    {
        public BlogsController(TranMinhKhoiDbContext context, INotyfService notyfService, IConfiguration configuration) : base(context, notyfService, configuration)
        {
        }


        // GET: Admin/Blogs
        public async Task<IActionResult> IndexPage()
        {
            var tranMinhKhoiDbContext = _context.Blogs.Include(b => b.Account).Where(x => 
            x.Type != "Blog" && 
            x.Type != "Social" &&
            x.Type != "Politics" && 
            x.Type != "IELTS" && 
            x.Type != "SoftSkill" && 
            x.Type != "Competion"
            );
            return View(await tranMinhKhoiDbContext.ToListAsync());
        }
        public async Task<IActionResult> Index()
        {
            var tranMinhKhoiDbContext = _context.Blogs.Include(b => b.Account).Where(x => x.Type == "Blog");
            return View(await tranMinhKhoiDbContext.ToListAsync());
        }

        public async Task<IActionResult> IndexSocial()
        {
            var tranMinhKhoiDbContext = _context.Blogs.Include(b => b.Account).Where(x => x.Type == "Social");
            return View(await tranMinhKhoiDbContext.ToListAsync());
        }
        public async Task<IActionResult> IndexPolitics()
        {
            var tranMinhKhoiDbContext = _context.Blogs.Include(b => b.Account).Where(x => x.Type == "Politics");
            return View(await tranMinhKhoiDbContext.ToListAsync());
        }
        public async Task<IActionResult> IndexCompetion()
        {
            var tranMinhKhoiDbContext = _context.Blogs.Include(b => b.Account).Where(x => x.Type == "Competion");
            return View(await tranMinhKhoiDbContext.ToListAsync());
        }
          public async Task<IActionResult> IndexIELTS()
        {
            var tranMinhKhoiDbContext = _context.Blogs.Include(b => b.Account).Where(x => x.Type == "IELTS");
            return View(await tranMinhKhoiDbContext.ToListAsync());
        }
            public async Task<IActionResult> IndexSoftSkill()
        {
            var tranMinhKhoiDbContext = _context.Blogs.Include(b => b.Account).Where(x => x.Type == "SoftSkill");
            return View(await tranMinhKhoiDbContext.ToListAsync());
        }

        // GET: Admin/Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog, IFormFile fAvatar)
        {

            //var makhclaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            //if (makhclaim == null)
            //{
            //    _notyfService.Error("Bạn chưa đăng nhập, vui lòng đăng nhập để thực hiện chức năng này.");
            //    return View();
            //}
            //var maKH = makhclaim.Value;
            if (fAvatar != null)
            {
                string extennsion = Path.GetExtension(fAvatar.FileName);
                image = Utilities.ToUrlFriendly(blog.Title ?? "") + extennsion;
                blog.Image = await Utilities.UploadFile(fAvatar, @"Blog", image.ToLower());
            }
            blog.Cdt = DateTime.UtcNow.AddHours(7);
            blog.AccountId = null;
            blog.Status = 1;
            _context.Add(blog);
            await _context.SaveChangesAsync();

            if (blog?.Type == "Blog")
            {
                return RedirectToAction(nameof(Index));

            }
            else if (blog?.Type == "Social")
            {
                return RedirectToAction(nameof(IndexSocial));
            }
            else if (blog?.Type == "Politics")
            {
                return RedirectToAction(nameof(IndexPolitics));

            }
            else if (blog?.Type == "Competion")
            {
                return RedirectToAction(nameof(IndexCompetion));

            }  else if (blog?.Type == "IELTS")
            {
                return RedirectToAction(nameof(IndexIELTS));

            } 
            else if (blog?.Type == "SoftSkill")
            {
                return RedirectToAction(nameof(IndexSoftSkill));

            }
            else if (blog?.Type == "Privacy-Policy")
            {
                return RedirectToAction(nameof(IndexPage));

            }
            else if (blog?.Type == "Profile")
            {
                return RedirectToAction(nameof(IndexPage));

            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Blog blog, IFormFile fAvatar)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            try
            {
                if (fAvatar != null)
                {
                    string extennsion = Path.GetExtension(fAvatar.FileName);
                    image = Utilities.ToUrlFriendly(blog.Title ?? "") + extennsion;
                    blog.Image = await Utilities.UploadFile(fAvatar, @"Blog", image.ToLower());
                }
                else
                {
                    blog.Image = _context.Blogs.Where(x => x.Id == id).Select(x => x.Image).FirstOrDefault();
                }

                _context.Update(blog);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(blog.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            if (blog?.Type == "StudentLife-2" || blog?.Type == "StudentLife-1" || blog?.Type == "Profile")
            {
                return RedirectToAction(nameof(IndexPage));

            }
            else if (blog?.Type == "Blog")
            {
                return RedirectToAction(nameof(Index));

            }
            else if (blog?.Type == "Social")
            {
                return RedirectToAction(nameof(IndexSocial));
            }
            else if (blog?.Type == "Politics")
            {
                return RedirectToAction(nameof(IndexPolitics));

            }
            else if (blog?.Type == "Competion")
            {
                return RedirectToAction(nameof(IndexCompetion));

            }
            else if (blog?.Type == "Privacy-Policy")
            {
                return RedirectToAction(nameof(IndexPage));

            }
            else if (blog?.Type == "Profile")
            {
                return RedirectToAction(nameof(IndexPage));

            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Admin/Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
            }

            await _context.SaveChangesAsync();
            if (blog?.Type == "Blog")
            {
                return RedirectToAction(nameof(Index));

            }
            else if (blog?.Type == "Social")
            {
                return RedirectToAction(nameof(IndexSocial));
            }
            else if (blog?.Type == "Politics")
            {
                return RedirectToAction(nameof(IndexPolitics));

            }
            else if (blog?.Type == "Competion")
            {
                return RedirectToAction(nameof(IndexCompetion));

            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
