var userId = document.getElementById('UserId').value;
var connection = new signalR.HubConnectionBuilder().withUrl("/SignalRHub?userId=" + userId).build();

connection.start().then(function () {
    console.log("SignalR Connected!");
}).catch(function (err) {
    return console.error(err.toString());
});
connection.on("UserConnected", function (ConnectionId) {

    toastr.success("User connected with AccountId: " + ConnectionId, "Kết nối thành công");
});

connection.on("UserDisconnected", function (ConnectionId) {
    console.log("User disconnected with AccountId: " + ConnectionId);
});


connection.on("ReceivePrivateMessage", function (message) {
    if (message) {
        toastr.success("Thanh toán thanh công");
    }
    else {
        toastr.error("Thanh toán thất bại");
    }
});
