using Fleck;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    [SerializeField] protected MessageManager _messageManager;
    [SerializeField] protected TeamManager _teamManager;
    [SerializeField] protected UIManager _uiManager;

    public virtual void Initialize(MinigameData minigameData)
    {
        _messageManager.OnMessageReceived += ReceiveMessage;
        gameObject.SetActive(true);
        _uiManager.ShowGameSelection(false);
    }

    protected virtual void ReceiveMessage(IWebSocketConnection socket, string message)
    {
    }

    protected virtual void SendMessageToServer(string message)
    {
        _messageManager.SendMessageToServer(message);
    }

    protected virtual void FinishGame()
    {
        _messageManager.OnMessageReceived -= ReceiveMessage;
        gameObject.SetActive(false);
        _uiManager.ShowGameSelection(true);
        SendMessageToServer("home");
    }
}
