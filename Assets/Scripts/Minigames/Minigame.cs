using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    [SerializeField] private MessageReceiver _messageReceiver;

    public virtual void Initialize()
    {
        _messageReceiver.OnMessageReceived += ReceiveMessage;
        gameObject.SetActive(true);
    }

    protected virtual void ReceiveMessage(string message)
    {

    }

    protected virtual void Play()
    {

    }

    protected virtual void FinishGame()
    {
        _messageReceiver.OnMessageReceived -= ReceiveMessage;
        gameObject.SetActive(false);
    }
}
