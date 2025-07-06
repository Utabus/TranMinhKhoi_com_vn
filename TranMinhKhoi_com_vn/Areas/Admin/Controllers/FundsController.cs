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
    public class FundsController : BaseController
    {
        public FundsController(TranMinhKhoiDbContext context, INotyfService notyfService, IConfiguration configuration) : base(context, notyfService, configuration)
        {
        }



        // GET: Admin/Funds
        public async Task<IActionResult> Index()
        {
            var tranMinhKhoiDbContext = _context.Funds.Include(f => f.Account);
            return View(await tranMinhKhoiDbContext.ToListAsync());
        }

        // GET: Admin/Funds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fund = await _context.Funds
                .Include(f => f.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fund == null)
            {
                return NotFound();
            }

            return View(fund);
        }

        // GET: Admin/Funds/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id");
            return View();
        }

        // POST: Admin/Funds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Total,InOut,Cdt,Status,Content,AccountId")] Fund fund)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fund);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", fund.AccountId);
            return View(fund);
        }

        // GET: Admin/Funds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fund = await _context.Funds.FindAsync(id);
            if (fund == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", fund.AccountId);
            return View(fund);
        }

        // POST: Admin/Funds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Total,InOut,Cdt,Status,Content,AccountId")] Fund fund)
        {
            if (id != fund.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fund);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FundExists(fund.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", fund.AccountId);
            return View(fund);
        }

        // GET: Admin/Funds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fund = await _context.Funds
                .Include(f => f.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fund == null)
            {
                return NotFound();
            }

            return View(fund);
        }

        // POST: Admin/Funds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fund = await _context.Funds.FindAsync(id);
            if (fund != null)
            {
                _context.Funds.Remove(fund);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FundExists(int id)
        {
            return _context.Funds.Any(e => e.Id == id);
        }
    }
}
