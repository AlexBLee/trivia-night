using System.Collections.Generic;
using Fleck;
using UnityEngine;

public class Server : MonoBehaviour
{
    private WebSocketServer _server;
    private List<IWebSocketConnection> _clients = new();

    // keeping a persistent list for any possible disconnect/recoveries.
    private Dictionary<string, IWebSocketConnection> _clientHistory = new();

    public void Start()
    {
        _server = new WebSocketServer("ws://0.0.0.0:8080");

        _server.Start(socket =>
        {
            socket.OnOpen = () =>
            {
                Debug.Log($"Client connected: {socket.ConnectionInfo.ClientIpAddress}");
                _clients.Add(socket);
                _clientHistory.Add(socket.ConnectionInfo.ClientIpAddress, socket);
            };

            socket.OnClose = () =>
            {
                Debug.Log("Client Disconnected");
                _clients.Remove(socket);
            };

            socket.OnMessage = message =>
            {
                Debug.Log("Server message: " + message);
            };

            socket.OnError = (e) =>
            {
                Debug.LogError("Exception: " + e);
                _clients.Remove(socket);
                socket.Close();
            };
        });
    }

    private void OnDestroy()
    {
        foreach (var client in _clients)
        {
            client.Send("Closing.");
            client.Close();
        }

        _clients.Clear();
        _clientHistory.Clear();
        _server.Dispose();
    }
}
