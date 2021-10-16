var connection = new signalR.HubConnectionBuilder().withUrl("/Chat/Index").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message) {
    var name = document.getElementById("creatorName").innerText;
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
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});