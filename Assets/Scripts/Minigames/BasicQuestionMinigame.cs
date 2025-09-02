using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicQuestionMinigame : Minigame
{
    [SerializeField] private Button _finishButton;
    [SerializeField] private TextMeshProUGUI _questionText;

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessage("question");
        _finishButton.onClick.AddListener(FinishGame);

        _questionText.text = minigameData.Input;
    }

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
