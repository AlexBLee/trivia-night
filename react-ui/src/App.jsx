import { useWebSocket } from './WebsocketServer';
import './App.css'
import Home from './Home';
import Hangman from './Hangman';
import QuestionMinigame from './QuestionMinigame';
import MusicGame from './MusicGame';
import GeoguessrGame from './GeoguessrGame';
import ZoomInMinigame from './ZoomInMinigame';
import Lobby from './Lobby';
import ChimpTestGame from './ChimpTestGame';

export default function App() {
  const { view } = useWebSocket();

  const views = {
    lobby: <Lobby />,
    home: <Home />,
    hangman: <Hangman />,
    question: <QuestionMinigame />,
    music: <MusicGame />,
    geoguessr: <GeoguessrGame />,
    zoomin: <ZoomInMinigame/>,
    chimpTest: <ChimpTestGame/>
  };

  return (
    <div>
      <h2>Current View: {view}</h2>
      {views[view] || <h1>❓ Unknown view</h1>}
    </div>
  );
}
