$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/counterHub").build();
    connection.start().then(function () {

    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("UpdateCount", function (counter) {
        window.sessionStorage.setItem('OnlineUser', counter)
    });
})