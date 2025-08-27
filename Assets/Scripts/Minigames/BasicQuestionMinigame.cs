using UnityEngine;
using UnityEngine.UI;

public class BasicQuestionMinigame : Minigame
{
    [SerializeField] private Button _finishButton;

    public override void Initialize()
    {
        base.Initialize();
        SendMessage("question");
        _finishButton.onClick.AddListener(FinishGame);
    }

    [ContextMenu("AAAA")]
    public void ReenableGuess()
    {
        SendMessage("reenable");
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
