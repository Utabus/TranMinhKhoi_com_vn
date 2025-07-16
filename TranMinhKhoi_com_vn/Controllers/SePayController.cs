using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TranMinhKhoi_com_vn.Entities;
using TranMinhKhoi_com_vn.Helper;
using TranMinhKhoi_com_vn.Hubs;
using TranMinhKhoi_com_vn.Models;

namespace TranMinhKhoi_com_vn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SePayController : Controller
    {
        private readonly SignalR_Hub _signalR_Hub;
        private readonly TranMinhKhoiDbContext _context;
        public SePayController(SignalR_Hub signalR_Hub, TranMinhKhoiDbContext tranMinhKhoiDbContext)
        {
            _signalR_Hub = signalR_Hub;
            _context = tranMinhKhoiDbContext;
        }
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost("transaction")]
        [ApiKeyAuth]
        public IActionResult ReceiveTransaction([FromBody] JsonElement json)
        {
            var transaction = JsonSerializer.Deserialize<SePayTransaction>(json.GetRawText());
            var Email = transaction.description?.Split(" ")[1];
            if (transaction == null || string.IsNullOrEmpty(transaction.gateway) || transaction.transferAmount <= 0)
            {
                return BadRequest("Dữ liệu giao dịch không hợp lệ.");
            }
            var account = _context.Accounts.FirstOrDefault(a => a.Email == Email);
            if (account == null)
            {
                return NotFound($"Không tìm thấy tài khoản với email {Email}");
            }
            account.Coin +=transaction.transferAmount;
            _context.Accounts.Update(account);
            var history = new Fund
            {
                AccountId = account.Id,
                Total = transaction.transferAmount,
                InOut = true,
                Cdt = DateTime.UtcNow.AddHours(7),
                Content = transaction.description,
                Status = "Nạp tiền ngân hàng",
            };
            _context.Funds.Add(history);
            _context.SaveChanges();

            _signalR_Hub.SendPrivateMessage(account.Id.ToString(), true).Wait();
            return Ok();
        }
    }
}
