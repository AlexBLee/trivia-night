import { useState, useEffect } from 'react';
import { useWebSocket } from './WebsocketServer';
import './App.css';

function ZoomInMinigame() {
  const [isDisabled, setIsDisabled] = useState(false);
  const { socket, addMessageListener } = useWebSocket();
  
  useEffect(() => {
    if (!addMessageListener) return;

    const handleMessage = (message, event) => {
      console.log("ZoomInMinigame got:", message);

      if (message === "reenable") {
        setIsDisabled(false);
      }
    };

    const cleanup = addMessageListener(handleMessage);

    return cleanup;
  }, [addMessageListener]);

  const handleClick = (data) => {
    console.log("Button clicked");
    setIsDisabled(true);
    
    if (socket && socket.readyState === WebSocket.OPEN) {
      socket.send("button_clicked");
    }
  };

  return (
    <>
      <h1>Zoom In!</h1>
      <div className="card">
        <button onClick={handleClick} disabled={isDisabled}>
          Guess!
        </button>
      </div>
    </>
  );
}

export default ZoomInMinigame;