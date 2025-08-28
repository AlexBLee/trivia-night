using UnityEngine;
using UnityEngine.UI;

public class ZoomInMinigame : Minigame
{
    [SerializeField] private Button _finishButton;

    public override void Initialize()
    {
        base.Initialize();
        SendMessage("zoomin");
        _finishButton.onClick.AddListener(FinishGame);
    }

    protected override void ReceiveMessage(string message)
    {
        base.ReceiveMessage(message);
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        _finishButton.onClick.RemoveListener(FinishGame);
    }
}
