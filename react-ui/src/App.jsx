import { WebSocketServer, useWebSocket } from './WebsocketServer';
import './App.css'
import Home from './Home';
import Hangman from './Hangman';
import QuestionMinigame from './QuestionMinigame';
import MusicGame from './MusicGame';

function App() {
  const { view } = useWebSocket();

  const views = {
    home: <Home />,
    hangman: <Hangman />,
    question: <QuestionMinigame />,
    music: <MusicGame />,
  };

  return (
    <div>
      <h2>Current View: {view}</h2>
      {views[view] || <h1>❓ Unknown view</h1>}
    </div>
  );
}

export default App
