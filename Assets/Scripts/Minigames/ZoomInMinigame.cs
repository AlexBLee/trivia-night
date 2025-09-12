using Fleck;
using UnityEngine;
using UnityEngine.UI;

public class ZoomInMinigame : Minigame
{
    [SerializeField] private Button _finishButton;
    [SerializeField] private Image _image;

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessageToServer("zoomin");
        _finishButton.onClick.AddListener(FinishGame);

        var image = Resources.Load<Sprite>(minigameData.Input);
        _image.sprite = image;
    }

    protected override void ReceiveMessage(IWebSocketConnection socket, string message)
    {
        base.ReceiveMessage(socket, message);
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        _finishButton.onClick.RemoveListener(FinishGame);
    }
}
