var connection = new signalR.HubConnectionBuilder().withUrl("/Chat/Index").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message, name) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${name} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    document.getElementById("messageInput").value = "";
    var name = $("#creatorName").val();
    connection.invoke("SendMessage", message, name).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});