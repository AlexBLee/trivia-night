import './App.css'
import { useWebSocket } from './WebsocketServer';
import { useState } from 'react';

function Lobby() {
    const [value, setValue] = useState('');
    const { socket } = useWebSocket();

    const handleChange = (e) => {
        setValue(e.target.value);
    };

    const handleClick = () => {
        if (socket && socket.readyState === WebSocket.OPEN) {
            socket.send(`teamname:${value}`);
        }
    };

    return (
    <>
    <h2>Choose a team name!</h2>

    <input
        type="text"
        value={value}
        onChange={handleChange}
        placeholder="Enter text here" />

    <button onClick={handleClick}>
        Submit!
    </button>
    </>
    
  )
}

export default Lobby