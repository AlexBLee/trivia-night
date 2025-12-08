using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicQuestionMinigame : SingleGuessMinigame
{
    [SerializeField] private Button _finishButton;
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private TextMeshProUGUI _answerText;
    public Action<Team, int> OnGuessClicked { get; set; }

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessageToServer("question");
        _finishButton.onClick.AddListener(FinishGame);

        _questionText.text = minigameData.Input;
        _answerText.text = minigameData.Answer;

        _uiManager.ShowCharacters();

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
        SendMessageToServer("reenable");
    }

    private void RevealAnswer()
    {
        _answerText.gameObject.SetActive(true);
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        _uiManager.ShowCharacters(false);
        _finishButton.onClick.RemoveListener(FinishGame);
    }
}
