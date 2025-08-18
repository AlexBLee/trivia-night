const socket = new WebSocket("ws://192.168.1.77:8080");

socket.addEventListener("open", (event) => {
    socket.send("Hello Server!");
});

socket.addEventListener("message", (event) => {

    if (event.data === "hangman")
    {
        console.log("HANGMAN!");
        document.body.innerHTML = `
        <h1>Hangman Game</h1>
        <div id="gameArea"></div>
    `;
    }
});

// Fired if the connection closes
socket.addEventListener("close", (event) => {
});

// Fired if there’s an error
socket.addEventListener("error", (error) => {
});