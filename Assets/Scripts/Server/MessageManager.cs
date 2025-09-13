using System;
using Fleck;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private Server _server;

    private string _lastMessageReceived;
    public event Action<IWebSocketConnection, string> OnMessageReceived;

    public void SendMessageToServer(string message)
    {
        _server.SendMessageToAll(message);
    }

    public void ReceiveMessage(IWebSocketConnection socket, string message)
    {
        OnMessageReceived?.Invoke(socket, message);
    }
}
