using Microsoft.AspNetCore.SignalR;

namespace TranMinhKhoi_com_vn.Hubs
{
    public class QueryStringUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.GetHttpContext()?.Request.Query["userId"];
        }
    }
}
