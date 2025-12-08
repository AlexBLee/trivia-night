using DG.Tweening;
using Fleck;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    [SerializeField] protected MessageManager _messageManager;
    [SerializeField] protected TeamManager _teamManager;
    [SerializeField] protected UIManager _uiManager;
    [SerializeField] protected float _scaleAnimationTime = 0.35f;
    [SerializeField] protected int _points = 0;

    public virtual void Initialize(MinigameData minigameData)
    {
        _messageManager.OnMessageReceived += ReceiveMessage;
        _points = minigameData.Points;
        gameObject.SetActive(true);
        gameObject.transform.localScale = Vector3.zero;
        gameObject.transform.DOScale(Vector3.one, _scaleAnimationTime);
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
        gameObject.transform.DOScale(Vector3.zero, _scaleAnimationTime).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        _uiManager.ShowGameSelection(true);
        SendMessageToServer("home");
    }
}
