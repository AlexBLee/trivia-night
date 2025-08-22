import { createContext, useContext, useEffect, useState } from "react";

const WebSocketContext = createContext();

export function WebSocketServer({ children })
{
    const [view, setView] = useState("home");
    const [socket, setSocket] = useState(null);

    useEffect(() =>
    {
        const socket = new WebSocket("ws://192.168.1.77:8080");

        socket.onopen = () => {
            socket.send("Hello Server!");
        };

        socket.onmessage = (event) => {
            const message = event.data;
            console.log("Received:", message);

            if (message === "home") setView("home");
            if (message === "question") setView("question");
            if (message === "hangman") setView("hangman");
            if (message === "music") setView("music");
        };

        socket.onclose = () => {
        console.log("WebSocket closed");
        };

        setSocket(socket);

        return () => socket.close();
    }, []);

    return (
        <WebSocketContext.Provider value={{socket, view, setView}}>
            {children}
        </WebSocketContext.Provider>
    )
}

export function useWebSocket() {
    return useContext(WebSocketContext);
}

