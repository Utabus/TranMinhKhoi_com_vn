var userId = document.getElementById('UserId').value;
var connection = new signalR.HubConnectionBuilder().withUrl("/SignalRHub?userId=" + userId).build();

connection.start().then(function () {
    console.log("SignalR Connected!");
}).catch(function (err) {
    return console.error(err.toString());
});
connection.on("UserConnected", function (ConnectionId) {
    //console.log("User disconnected with AccountId: " + ConnectionId);
    //toastr.success("User connected with AccountId: " + ConnectionId, "Kết nối thành công");
});

connection.on("UserDisconnected", function (ConnectionId) {
    console.log("User disconnected with AccountId: " + ConnectionId);
});



connection.on("ReceivePrivateMessage", function (message, total) {
    if (message) {
        // Format số tiền nhận được
        var amount = Number(total);

        // Lấy số dư hiện tại từ thẻ
        var balanceElement = document.getElementById("balanceAmount");
        var current = parseInt(balanceElement.innerText.replace(/\D/g, '')) || 0;

        // Cộng thêm
        var newBalance = current + amount;

        // Format về dạng "200,000"
        balanceElement.innerText = newBalance.toLocaleString('vi-VN');

        toastr.success("Thanh toán thành công: " + amount.toLocaleString('vi-VN') + " ₫");
    }
    else {
        toastr.error("Thanh toán thất bại");
    }
});
