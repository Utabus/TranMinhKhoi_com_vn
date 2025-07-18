using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TranMinhKhoi_com_vn.Entities;

namespace TranMinhKhoi_com_vn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : BaseController
    {
        public HomeController(TranMinhKhoiDbContext context, INotyfService notyfService, IConfiguration configuration) : base(context, notyfService, configuration)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
