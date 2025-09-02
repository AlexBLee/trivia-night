using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    [SerializeField] protected MessageManager _messageManager;

    public virtual void Initialize(MinigameData minigameData)
    {
        _messageManager.OnMessageReceived += ReceiveMessage;
        gameObject.SetActive(true);
    }

    protected virtual void ReceiveMessage(string message)
    {

    }

    protected virtual void SendMessage(string message)
    {
        _messageManager.SendMessageToServer(message);
    }

    protected virtual void Play()
    {

    }

    protected virtual void FinishGame()
    {
        _messageManager.OnMessageReceived -= ReceiveMessage;
        gameObject.SetActive(false);
        SendMessage("home");
    }
}
