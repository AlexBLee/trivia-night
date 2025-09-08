import { useState, useEffect, useRef, useMemo, useCallback } from 'react';
import { useWebSocket } from './WebsocketServer';
import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
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

function DraggableMarker({position, setPosition}) {
  const [draggable, setDraggable] = useState(false)
  const markerRef = useRef(null)

  const eventHandlers = useMemo(
    () => ({
      dragend() {
        const marker = markerRef.current
        if (marker != null) {
          setPosition(marker.getLatLng())
        }
      },
    }),
    [setPosition],
  )

  const toggleDraggable = useCallback(() => {
    setDraggable((d) => !d)
  }, [])

  return (
    <Marker
      draggable={draggable}
      eventHandlers={eventHandlers}
      position={position}
      ref={markerRef}>
      <Popup minWidth={90}>
        <span onClick={toggleDraggable}>
          {draggable
            ? 'Marker is draggable'  
            : 'Click here to make marker draggable'}
        </span>
      </Popup>
    </Marker>
  )
}

function GeoguessrGame() {
  const { socket, addMessageListener } = useWebSocket();
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
    }
  };

  return (
    <>
      <h1>Geoguessr!</h1>
      <div className="map">
        <MapContainer center={[49.206289, -122.940801]} zoom={11}>
          <TileLayer url='https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png'/>
          <DraggableMarker position={markerPosition} setPosition={setMarkerPosition}/>
        </MapContainer>
      </div>

      <div className="position-display">
        <p>Marker Position:</p>
        <p>Latitude: {markerPosition.lat.toFixed(6)}</p>
        <p>Longitude: {markerPosition.lng.toFixed(6)}</p>
        <button onClick={() => 
          handleClick
          (markerPosition.lat.toFixed(6), 
          markerPosition.lng.toFixed(6))}>
            Submit</button>
      </div>
    </>
  );
}

export default GeoguessrGame;