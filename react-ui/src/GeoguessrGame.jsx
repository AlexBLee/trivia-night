import { useState, useEffect, useRef, useMemo, useCallback } from 'react';
import { useWebSocket } from './WebsocketServer';
import { MapContainer, TileLayer, Marker, useMapEvents } from 'react-leaflet';
import './App.css';
import "leaflet/dist/leaflet.css";
import L from "leaflet";

import markerIcon2x from "leaflet/dist/images/marker-icon-2x.png";
import markerIcon from "leaflet/dist/images/marker-icon.png";
import markerShadow from "leaflet/dist/images/marker-shadow.png";

delete L.Icon.Default.prototype._getIconUrl;

L.Icon.Default.mergeOptions({
  iconRetinaUrl: markerIcon2x,
  iconUrl: markerIcon,
  shadowUrl: markerShadow,
});

const center = {
  lat: 49.206289,
  lng: -122.940801,
}

function LocationMarker({position, setPosition}) {
  const map = useMapEvents({
    click(e) {
      setPosition(e.latlng);
    },
  });

  return position === null ? null : (
    <Marker position={position} draggable={true} eventHandlers={{
      dragend(e) {
        setPosition(e.target.getLatLng());
      }
    }}>
    </Marker>
  );
}

function GeoguessrGame() {
  const { socket, addMessageListener } = useWebSocket();
  const [submitText, setSubmitText] = useState('Submit Guess');
  const [markerPosition, setMarkerPosition] = useState(center);

  useEffect(() => {
    if (!addMessageListener) return;

    const handleMessage = (message, event) => {
      console.log("GeoguessrGame got:", message);
    };

    const cleanup = addMessageListener(handleMessage);

    return cleanup;
  }, [addMessageListener]);

  const handleClick = (lat, lng) => {
    if (socket && socket.readyState === WebSocket.OPEN) {
      socket.send(`${lat},${lng}`);
      setSubmitText('Submitted!')
    }
  };

  return (
    <>
      <h1>Geoguessr!</h1>
      <div className="map-fullscreen">
        <MapContainer center={[49.206289, -122.940801]} zoom={11}>
          <TileLayer url='https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png'/>
          <LocationMarker position={markerPosition} setPosition={setMarkerPosition}/>
        </MapContainer>
        
        <button 
          className="submit-button-overlay"
          onClick={() => handleClick(
            markerPosition.lat.toFixed(6), 
            markerPosition.lng.toFixed(6)
          )}>
          {submitText}
        </button>
      </div>
    </>
  );
}

export default GeoguessrGame;