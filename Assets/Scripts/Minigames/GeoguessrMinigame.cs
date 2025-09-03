using UnityEngine;
using UnityEngine.UI;

public class GeoguessrMinigame : Minigame
{
    [SerializeField] private Button _finishButton;
    [SerializeField] private Image _image;

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessage("geoguessr");
        _finishButton.onClick.AddListener(FinishGame);

        var image = Resources.Load<Sprite>(minigameData.Input);
        _image.sprite = image;
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
