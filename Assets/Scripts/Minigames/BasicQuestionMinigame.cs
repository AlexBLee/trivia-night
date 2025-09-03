using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicQuestionMinigame : Minigame
{
    [SerializeField] private Button _finishButton;
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private TextMeshProUGUI _answerText;

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessage("question");
        _finishButton.onClick.AddListener(FinishGame);

        _questionText.text = minigameData.Input;
        _answerText.text = minigameData.Answer;

        _answerText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReenableGuess();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            RevealAnswer();
        }
    }

    private void ReenableGuess()
    {
        SendMessage("reenable");
    }

    private void RevealAnswer()
    {
        _answerText.gameObject.SetActive(true);
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
