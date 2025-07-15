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
        private readonly TranMinhKhoiDbContext _tranMinhKhoiDbContext;
        public HomeController(ILogger<HomeController> logger ,TranMinhKhoiDbContext tranMinhKhoiDbContext)
        {
            _logger = logger;
            _tranMinhKhoiDbContext = tranMinhKhoiDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Social()
        {
            return View();
        }
        public IActionResult Politics()
        {
            return View();
        }
        public IActionResult Competion()
        {
            return View();
        }
        public async Task<IActionResult> Fund()
        {
            return View(await _tranMinhKhoiDbContext.KeySePays.FirstOrDefaultAsync());
        }
        public IActionResult StudentLife()
        {
            return View();
        }

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
