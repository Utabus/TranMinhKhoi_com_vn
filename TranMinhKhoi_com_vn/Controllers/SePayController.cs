using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TranMinhKhoi_com_vn.Helper;
using TranMinhKhoi_com_vn.Models;

namespace TranMinhKhoi_com_vn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SePayController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost("transaction")]
        [ApiKeyAuth]
        public IActionResult ReceiveTransaction([FromBody] JsonElement json)
        {
            var transaction = JsonSerializer.Deserialize<SePayTransaction>(json.GetRawText());
            // Log, xử lý lưu DB hoặc xử lý logic tùy bạn
            Console.WriteLine($"Nhận giao dịch từ {transaction.gateway} với số tiền {transaction.transferAmount:N0}");

            // Giả sử lưu DB thành công
            return Ok();
        }
    }
}
