import { useState } from "react";
import { useWebSocket } from './WebsocketServer';
import './App.css';

function KahootMinigame() {
  const { socket, addMessageListener } = useWebSocket();
  const [selected, setSelected] = useState(null);

    const colorToValue = {
    green: 0,
    red: 1,
    blue: 2,
    yellow: 3,
  };
  
  const sendMessage = (msg) => {
    if (selected === msg) return;
    setSelected(msg);

    const value = colorToValue[msg];

    if (socket && socket.readyState === WebSocket.OPEN) {
      socket.send(value);
    }
  };

  const Button = ({ color }) => (
    <button
      onClick={() => sendMessage(color)}
      style={{
        ...styles.button,
        background: color,
        transform: selected === color ? "translateY(4px)" : "translateY(0)",
        filter: selected === color ? "brightness(85%)" : "brightness(100%)",
      }}
    >
      {color}
    </button>
  );

  return (
    <div style={styles.container}>
      <Button color="green" />
      <Button color="red" />
      <Button color="blue" />
      <Button color="yellow" />
    </div>
  );
}

const styles = {
  container: {
    height: "100vh",
    display: "flex",
    flexDirection: "column",
  },
  button: {
    flex: 1,
    width: "100%",
    border: "none",
    fontSize: "2rem",
    color: "white",
    cursor: "pointer",
    transition: "all 0.1s ease-in-out",
  },
};


export default KahootMinigame;