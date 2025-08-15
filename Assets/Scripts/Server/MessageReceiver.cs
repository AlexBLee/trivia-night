using System;
using UnityEngine;

public class MessageReceiver : MonoBehaviour
{
    public event Action<string> OnMessageReceived;

    public void ReceiveMessage(string message)
    {
        OnMessageReceived?.Invoke(message);
    }
}
