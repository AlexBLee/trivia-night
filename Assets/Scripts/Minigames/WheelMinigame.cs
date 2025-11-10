using Fleck;
using UnityEngine;
using UnityEngine.UI;

public class WheelMinigame : Minigame
{
    [SerializeField] private Button _finishButton;

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessageToServer("gambling");
        _finishButton.onClick.AddListener(FinishGame);
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
