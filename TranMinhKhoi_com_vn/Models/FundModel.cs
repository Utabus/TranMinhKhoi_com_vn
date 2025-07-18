using TranMinhKhoi_com_vn.Entities;

namespace TranMinhKhoi_com_vn.Models
{
    public class FundModel
    {
        public virtual Account? Account { get; set; }
        public virtual KeySePay? KeySePay { get; set; }
    }
}
