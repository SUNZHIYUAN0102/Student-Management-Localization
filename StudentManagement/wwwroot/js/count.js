$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/counterHub").build();
    connection.start().then(function () {

    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("UpdateCount", function (counter) {
        var strong = document.getElementById("counter");
        strong.innerText = counter;
    });
})