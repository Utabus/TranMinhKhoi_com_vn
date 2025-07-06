using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using TranMinhKhoi_com_vn.Areas.Admin.Controllers;
using TranMinhKhoi_com_vn.Entities;

namespace TranMinhKhoi_com_vn.Controllers
{
	public class StudentLifeController : BaseController
	{
        public StudentLifeController(TranMinhKhoiDbContext context, INotyfService notyfService, IConfiguration configuration) : base(context, notyfService, configuration)
        {
        }

        public IActionResult Index()
		{
			return View();
		}
        public IActionResult StudentLife()
        {
            return View();
        }

        public IActionResult CoursesDetail()
        {
            return View();
        }

        public IActionResult Courses()
        {
            return View();
        }

       
    }
}
