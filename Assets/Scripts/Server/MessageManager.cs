using System;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private Server _server;
    public event Action<string> OnMessageReceived;

    public void SendMessageToServer(string message)
    {
        _server.SendMessageToAll(message);
    }

    public void ReceiveMessage(string message)
    {
        OnMessageReceived?.Invoke(message);
    }
}
