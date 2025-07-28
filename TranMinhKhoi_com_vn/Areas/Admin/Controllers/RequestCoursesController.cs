using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TranMinhKhoi_com_vn.Entities;

namespace TranMinhKhoi_com_vn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RequestCoursesController : BaseController
    {
        public RequestCoursesController(TranMinhKhoiDbContext context, INotyfService notyfService, IConfiguration configuration) : base(context, notyfService, configuration)
        {
        }


        // GET: Admin/RequestCourses
        public async Task<IActionResult> Index()
        {
            var tranMinhKhoiDbContext = _context.RequestCourses.Include(r => r.Account).Include(r => r.Course);
            return View(await tranMinhKhoiDbContext.ToListAsync());
        }

        // GET: Admin/RequestCourses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestCourse = await _context.RequestCourses
                .Include(r => r.Account)
                .Include(r => r.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestCourse == null)
            {
                return NotFound();
            }

            return View(requestCourse);
        }

        // GET: Admin/RequestCourses/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Email");
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,CourseId,Cdt,Status")] RequestCourse requestCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requestCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Email", requestCourse.AccountId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", requestCourse.CourseId);
            return View(requestCourse);
        }

        // GET: Admin/RequestCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestCourse = await _context.RequestCourses.FindAsync(id);
            if (requestCourse == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Email", requestCourse.AccountId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", requestCourse.CourseId);
            return View(requestCourse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,CourseId,Cdt,Status")] RequestCourse requestCourse)
        {
            if (id != requestCourse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requestCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestCourseExists(requestCourse.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Email", requestCourse.AccountId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", requestCourse.CourseId);
            return View(requestCourse);
        }

        // GET: Admin/RequestCourses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestCourse = await _context.RequestCourses
                .Include(r => r.Account)
                .Include(r => r.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestCourse == null)
            {
                return NotFound();
            }

            return View(requestCourse);
        }

        // POST: Admin/RequestCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requestCourse = await _context.RequestCourses.FindAsync(id);
            if (requestCourse != null)
            {
                _context.RequestCourses.Remove(requestCourse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestCourseExists(int id)
        {
            return _context.RequestCourses.Any(e => e.Id == id);
        }

        // POST: Admin/RequestCourses/Confirm/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id)
        {
            var requestCourse = await _context.RequestCourses.FindAsync(id);
            if (requestCourse == null)
            {
                return NotFound();
            }

            requestCourse.Status = true;
            _context.Update(requestCourse);
            await _context.SaveChangesAsync();

            // Nếu dùng NotyfService thì có thể thêm thông báo:
            _notyfService.Success("Yêu cầu đã được xác nhận!");

            return RedirectToAction(nameof(Index));
        }
    }
}
