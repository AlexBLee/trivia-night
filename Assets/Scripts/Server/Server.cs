using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Fleck;
using UnityEngine;

public class Server : MonoBehaviour
{
    [SerializeField] private MessageManager _messageManager;

    private WebSocketServer _server;
    private ConcurrentDictionary<string, IWebSocketConnection> _clients = new();

    // keeping a persistent list for any possible disconnect/recoveries.
    private Dictionary<string, IWebSocketConnection> _clientHistory = new();

    public Action<IWebSocketConnection> OnConnected;
    public Action<IWebSocketConnection> OnDisconnected;

    public void Start()
    {
        _server = new WebSocketServer($"ws://{ServerExtensions.GetLocalIpv4Address()}:8080");

        _server.Start(socket =>
        {
            socket.OnOpen = () => { OnClientConnected(socket); };
            socket.OnClose = () => { OnClientDisconnected(socket); };
            socket.OnMessage = (msg) => { OnMessageReceived(socket, msg); };
            socket.OnError = (e) => { OnClientError(socket, e); };
        });
    }

    private void OnClientConnected(IWebSocketConnection socket)
    {
        var clientId = GetClientId(socket);

        if (string.IsNullOrEmpty(clientId))
        {
            Debug.Log("No client ID provided, closing connection");
            socket.Close();
            return;
        }

        if (_clients.TryGetValue(clientId, out var existingSocket))
        {
            Debug.Log($"Closing existing connection for client: {clientId}");
            existingSocket.Close();
            _clients.TryRemove(clientId, out _);
        }


        OnConnected?.Invoke(socket);
        _clients.TryAdd(clientId, socket);
        _clientHistory.TryAdd(clientId, socket);
        Debug.Log($"Client connected: {clientId} (Total: {_clients.Count})");
    }

    private void OnClientDisconnected(IWebSocketConnection socket)
    {
        var clientId = GetClientId(socket);
        if (!string.IsNullOrEmpty(clientId))
        {
            OnDisconnected?.Invoke(socket);
            _clients.TryRemove(clientId, out _);
            Debug.Log($"Client disconnected: {clientId} (Total: {_clients.Count})");
        }
    }

    private void OnMessageReceived(IWebSocketConnection socket, string message)
    {
        Debug.Log("Server message: " + message);
        _messageManager.ReceiveMessage(socket, message);
    }

    private void OnClientError(IWebSocketConnection socket, Exception e)
    {
        var clientId = GetClientId(socket);
        Debug.Log($"Error from {clientId}: {e.Message}");
    }

    public void SendMessageToAll(string message)
    {
        foreach (var client in _clients)
        {
            client.Value.Send(message);
        }
    }
    
    private string GetClientId(IWebSocketConnection socket)
    {
        return socket.ConnectionInfo.ClientIpAddress;
    }

    private void OnDestroy()
    {
        foreach (var client in _clients)
        {
            client.Value.Send("Closing.");
            client.Value.Close();
        }

        _clients.Clear();
        _clientHistory.Clear();
        _server.Dispose();
    }
}
