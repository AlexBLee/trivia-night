import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import { WebSocketServer } from './WebsocketServer.jsx'

createRoot(document.getElementById('root')).render(
    <WebSocketServer>
      <App />
    </WebSocketServer>
)
