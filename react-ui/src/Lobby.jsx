import './App.css'
import { useWebSocket } from './WebsocketServer';
import { useState } from 'react';

function Lobby() {
    const [value, setValue] = useState('');
    const [heading, setHeading] = useState('Choose a team name!');
    const { socket } = useWebSocket();

    const handleChange = (e) => {
        setValue(e.target.value);
    };

    const handleClick = () => {
        if (socket && socket.readyState === WebSocket.OPEN) {
            socket.send(`teamname:${value}`);
            setHeading(`Current team name: ${value}`);
            
        }
    };

    return (
    <>
    <h1>Welcome to Trivia Night!</h1>
    <h2>{heading}</h2>

    <input
        type="text"
        value={value}
        onChange={handleChange}
        placeholder="Enter text here" />

    <button name onClick={handleClick}>
        Submit!
    </button>
    </>
    
  )
}

export default Lobby