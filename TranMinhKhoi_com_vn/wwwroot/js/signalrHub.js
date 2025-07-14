var userId = document.getElementById('UserId').value;
var connection = new signalR.HubConnectionBuilder().withUrl("/SignalRHub?userId=" + userId).build();

connection.start().then(function () {
    console.log("SignalR Connected!");
}).catch(function (err) {
    return console.error(err.toString());
});
connection.on("UserConnected", function (ConnectionId) {
   
    console.log("User connected with AccountId: " + ConnectionId);
});

connection.on("UserDisconnected", function (ConnectionId) {
    console.log("User disconnected with AccountId: " + ConnectionId);
});
