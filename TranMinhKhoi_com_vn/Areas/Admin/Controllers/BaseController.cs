using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using TranMinhKhoi_com_vn.Entities;

namespace TranMinhKhoi_com_vn.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected readonly TranMinhKhoiDbContext _context;
        protected static string image;
        public INotyfService _notyfService { get; }
        protected readonly IConfiguration _configuration;
        public BaseController(TranMinhKhoiDbContext context, INotyfService notyfService, IConfiguration configuration)
        {

            _context = context;
            _notyfService = notyfService;
            _configuration = configuration;
        }
    }
}
