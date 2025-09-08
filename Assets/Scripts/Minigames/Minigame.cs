using Fleck;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    [SerializeField] protected MessageManager _messageManager;
    [SerializeField] protected TeamManager _teamManager;

    public virtual void Initialize(MinigameData minigameData)
    {
        _messageManager.OnMessageReceived += ReceiveMessage;
        gameObject.SetActive(true);
    }

    protected virtual void ReceiveMessage(IWebSocketConnection socket, string message)
    {
    }

    protected virtual void SendMessage(string message)
    {
        _messageManager.SendMessageToServer(message);
    }

    protected virtual void FinishGame()
    {
        _messageManager.OnMessageReceived -= ReceiveMessage;
        gameObject.SetActive(false);
        SendMessage("home");
    }
}
