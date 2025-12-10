import './App.css'
import { useWebSocket } from './WebsocketServer';
import { useState } from 'react';
import ImageCarousel from './ImageCarousel';

function Lobby() {
    const [value, setValue] = useState('');
    const [heading, setHeading] = useState('Choose a team name!');
    const [selectedImage, setSelectedImage] = useState('Stickman');
    const { socket } = useWebSocket();

    // Define your images
    const images = [
        { name: 'Stickman', url: 'stickman-front.png' },
        { name: 'Slimey', url: 'slimey-front.png' },
        { name: 'Chimken', url: 'chimken-front.png' },
        { name: 'TopHatMan', url: 'tophatman-front.png' },
        { name: 'Ghosty', url: 'ghosty-front.png' },
        { name: 'Miku', url: 'miku-front.png' },
    ];

    const handleChange = (e) => {
        setValue(e.target.value);
    };

    const handleClick = () => {
        if (socket && socket.readyState === WebSocket.OPEN) {
            socket.send(`${value}:${selectedImage}`);
            setHeading(`Current team name: ${value}`);
            
        }
    };

    const handleImageSelect = (imageName) => {
        setSelectedImage(imageName);
    };

    return (
    <>
    <h1>Welcome to Trivia Night!</h1>
    <h2>{heading}</h2>

    <ImageCarousel images={images} onImageSelect={handleImageSelect} />
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