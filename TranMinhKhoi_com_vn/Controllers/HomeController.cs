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
        public HomeController(ILogger<HomeController> logger, TranMinhKhoiDbContext tranMinhKhoiDbContext)
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
            return View(await _context.Blogs.Where(c => c.Type == "Social").ToListAsync());
        }


        public async Task<IActionResult> Politics()
        {
            return View(await _context.Blogs.Where(c => c.Type == "Politics").ToListAsync());
        }
        public async Task<IActionResult> Competion()
        {
            return View(await _context.Blogs.Where(c => c.Type == "Competion").ToListAsync());
        }

        public async Task<IActionResult> BlogDetails(int id)
        {
            return View(await _context.Blogs.FirstOrDefaultAsync(c => c.Id == id));
        }


        public async Task<IActionResult> Fund()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userName = User.Claims.FirstOrDefault(x => x.Type == "UserName").Value;
            var fundModel = new FundModel
            {
                Account = await _context.Accounts.FirstOrDefaultAsync(c => c.UserName == userName),
                KeySePay = await _context.KeySePays.FirstOrDefaultAsync()
            };  
            return View(fundModel);
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
