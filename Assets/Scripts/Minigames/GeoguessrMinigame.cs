using UnityEngine;
using UnityEngine.UI;

public class GeoguessrMinigame : Minigame
{
    [SerializeField] private Button _finishButton;

    public override void Initialize()
    {
        base.Initialize();
        SendMessage("geoguessr");
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
