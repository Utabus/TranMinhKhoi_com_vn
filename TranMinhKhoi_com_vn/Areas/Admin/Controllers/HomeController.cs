using Microsoft.AspNetCore.Mvc;

namespace TranMinhKhoi_com_vn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
