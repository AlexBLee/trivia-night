import { createContext, useContext, useEffect, useState, useCallback, useRef } from "react";

const WebSocketContext = createContext();

export function WebSocketServer({ children }) {
    const [view, setView] = useState("lobby");
    const [socket, setSocket] = useState(null);
    const messageListenersRef = useRef(new Set());

    // Function to add a message listener
    const addMessageListener = useCallback((callback) => {
        messageListenersRef.current.add(callback);
        
        // Return cleanup function
        return () => {
            messageListenersRef.current.delete(callback);
        };
    }, []);

    useEffect(() => {
        console.log("Connecting to Websocket..");
        console.log(window.location.hostname);
        const socket = new WebSocket(`ws://${window.location.hostname}:8080`);

        socket.onopen = () => {
        };

        socket.onmessage = (event) => {
            const message = event.data;
            console.log("Received:", message);

            // Handle view changes in the main component
            if (message === "lobby") setView("lobby");
            if (message === "home") setView("home");
            if (message === "question") setView("question");
            if (message === "hangman") setView("hangman");
            if (message === "music") setView("music");
            if (message === "geoguessr") setView("geoguessr");
            if (message === "zoomin") setView("zoomin");

            // Notify all registered listeners
            messageListenersRef.current.forEach(callback => {
                try {
                    callback(message, event);
                } catch (error) {
                    console.error("Error in message listener:", error);
                }
            });
        };

        socket.onclose = (event) => {
            console.log("WebSocket closed", event.code, event.reason);
        };

        setSocket(socket);

        return () => socket.close();
    }, []); // No dependencies - only run once

    return (
        <WebSocketContext.Provider value={{ 
            socket, 
            view, 
            setView, 
            addMessageListener 
        }}>
            {children}
        </WebSocketContext.Provider>
    );
}

export function useWebSocket() {
    return useContext(WebSocketContext);
}