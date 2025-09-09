using System.Linq;
using Fleck;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomizedWordMinigame : Minigame
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _finishButton;

    private string _word;

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessage("randomize");

        _word = minigameData.Answer;
        _text.text = minigameData.Input;

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
