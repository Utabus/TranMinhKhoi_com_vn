using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> StudentLifeDetail(string Type)
        {
            return View(await _context.Blogs.FirstOrDefaultAsync(c => c.Type == Type.Trim()));
        }

        public async Task<IActionResult> CoursesDetail(int id)
        {
            return View(await _context.Courses.FirstOrDefaultAsync(c => c.Id == id));
        }

        public async Task<IActionResult> Courses()
        {
            return View(await _context.Courses.ToListAsync());
        }

       
    }
}
