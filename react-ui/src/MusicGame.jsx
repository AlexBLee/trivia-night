import { useWebSocket } from './WebsocketServer'
import './App.css'

function MusicGame() {
  const { socket } = useWebSocket();

  const handleClick = () => {
    if (socket && socket.readyState == WebSocket.OPEN)
    {
      socket.send("turn");
    }
  }

  return (
    <>
      <h1>Question!</h1>
      <div className="card">
        <button onClick={handleClick}>
        </button>
      </div>
    </>
  )
}

export default MusicGame
