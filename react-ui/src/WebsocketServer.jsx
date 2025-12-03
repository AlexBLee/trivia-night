import { createContext, useContext, useEffect, useState, useCallback, useRef } from "react";

const WebSocketContext = createContext();

export function WebSocketServer({ children }) {
    const [view, setView] = useState("lobby");
    const [socket, setSocket] = useState(null);

    const reconnectDelayRef = useRef(1000);
    const reconnectTimeoutRef = useRef(null);
    const messageListenersRef = useRef(new Set());

    const fakeId = useRef(null);

    useEffect(() => {
        const savedId = sessionStorage.getItem("clientId");
        if (savedId) {
            fakeId.current = savedId;
        } else {
            const newId = "player_" + Math.floor(Math.random() * 100000);
            fakeId.current = newId;
            sessionStorage.setItem("clientId", newId);
        }
    }, []);

    // Function to add a message listener
    const addMessageListener = useCallback((callback) => {
        messageListenersRef.current.add(callback);
        
        return () => {
            messageListenersRef.current.delete(callback);
        };
    }, []);

    const connect = useCallback(() => {
        console.log("Connecting to Websocket..");
        console.log(window.location.hostname);

        const socket = new WebSocket(`ws://${window.location.hostname}:8080/?id=${fakeId.current}`);

        socket.onopen = () => {
            console.log("WebSocket connected!");

            reconnectDelayRef.current = 1000;

            if (reconnectTimeoutRef.current) {
                clearTimeout(reconnectTimeoutRef.current);
                reconnectTimeoutRef.current = null;
            }
        };

        socket.onmessage = (event) => {
            const message = event.data;
            console.log("Received:", message);

            setView((prev) => {
                switch (message) {
                    case "lobby": return "lobby";
                    case "home": return "home";
                    case "question": return "question";
                    case "hangman": return "hangman";
                    case "music": return "music";
                    case "geoguessr": return "geoguessr";
                    case "zoomin": return "zoomin";
                    case "chimpTest": return "chimpTest";
                    case "kahoot": return "kahoot";
                    default: return prev;
                }
            });

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

            if (view === "lobby")
            {
                return;
            }

            reconnectTimeoutRef.current = setTimeout(() => {
                connect();
                reconnectDelayRef.current = Math.min(10000, reconnectDelayRef.current * 1.5);
            }, reconnectDelayRef.current);
        };

        setSocket(socket);

        return () => socket.close();
    }, []);

    useEffect(() => {
        connect();
        return () => {
            if (reconnectTimeoutRef.current) clearTimeout(reconnectTimeoutRef.current);
            if (socket) socket.close();
        };
    }, []);

    useEffect(() => {
        const onVisible = () => {
            if (document.visibilityState === "visible") {
                console.log("Tab visible again — ensuring WebSocket is connected.");
                if (!socket || socket.readyState !== WebSocket.OPEN) {
                    connect();
                }
            }
        };

        document.addEventListener("visibilitychange", onVisible);
        return () => document.removeEventListener("visibilitychange", onVisible);
    });


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