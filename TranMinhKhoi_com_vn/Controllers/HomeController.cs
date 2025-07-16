using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TranMinhKhoi_com_vn.Entities;
using TranMinhKhoi_com_vn.Models;

namespace TranMinhKhoi_com_vn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TranMinhKhoiDbContext _context;
        public HomeController(ILogger<HomeController> logger ,TranMinhKhoiDbContext tranMinhKhoiDbContext)
        {
            _logger = logger;
            _context = tranMinhKhoiDbContext;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            return View(await _context.Blogs.FirstOrDefaultAsync(c => c.Type == "Profile"));
        }
        public async Task<IActionResult> Social()
        {
            return View(await _context.Blogs.FirstOrDefaultAsync(c => c.Type == "Social"));
        }
        public async Task<IActionResult> Politics()
        {
            return View(await _context.Blogs.FirstOrDefaultAsync(c => c.Type == "Politics"));
        }
        public async Task<IActionResult> Competion()
        {
            return View(await _context.Blogs.FirstOrDefaultAsync(c => c.Type == "Competion"));
        }
        public async Task<IActionResult> Fund()
        {
            return View(await _context.KeySePays.FirstOrDefaultAsync());
        }
        //public async Task<IActionResult> StudentLife()
        //{
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
