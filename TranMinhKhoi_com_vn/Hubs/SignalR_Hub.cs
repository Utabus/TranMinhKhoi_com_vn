using Microsoft.AspNetCore.SignalR;

namespace TranMinhKhoi_com_vn.Hubs
{
    public class SignalR_Hub :Hub
    {

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendPrivateMessage(string userId, bool message,decimal total)
        {
            await Clients.User(userId).SendAsync("ReceivePrivateMessage", message, total);
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context?.User?.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            var connectionId = Context?.ConnectionId;
            await Clients.User(userId).SendAsync("UserConnected", connectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context?.User?.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;

            var connectionId = Context?.ConnectionId;

            await Clients.User(userId).SendAsync("UserDisconnected", connectionId);

            await base.OnDisconnectedAsync(exception);
        }

    }
}
