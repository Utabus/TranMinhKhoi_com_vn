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

namespace TranMinhKhoi_com_vn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KeySePaysController : BaseController
    {
        public KeySePaysController(TranMinhKhoiDbContext context, INotyfService notyfService, IConfiguration configuration)
            : base(context, notyfService, configuration)
        {
        }




        // GET: Admin/KeySePays
        public async Task<IActionResult> Index()
        {
            return View(await _context.KeySePays.ToListAsync());
        }

        // GET: Admin/KeySePays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keySePay = await _context.KeySePays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (keySePay == null)
            {
                return NotFound();
            }

            return View(keySePay);
        }

        // GET: Admin/KeySePays/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KeySePay keySePay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(keySePay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(keySePay);
        }

        // GET: Admin/KeySePays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keySePay = await _context.KeySePays.FindAsync(id);
            if (keySePay == null)
            {
                return NotFound();
            }
            return View(keySePay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KeySePay keySePay)
        {
            if (id != keySePay.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(keySePay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeySePayExists(keySePay.Id))
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
            return View(keySePay);
        }

        // GET: Admin/KeySePays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keySePay = await _context.KeySePays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (keySePay == null)
            {
                return NotFound();
            }

            return View(keySePay);
        }

        // POST: Admin/KeySePays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var keySePay = await _context.KeySePays.FindAsync(id);
            if (keySePay != null)
            {
                _context.KeySePays.Remove(keySePay);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KeySePayExists(int id)
        {
            return _context.KeySePays.Any(e => e.Id == id);
        }
    }
}
