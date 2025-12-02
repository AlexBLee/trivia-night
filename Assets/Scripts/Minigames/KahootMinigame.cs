using System.Collections.Generic;
using Fleck;
using UnityEngine;
using UnityEngine.UI;

public class KahootMinigame : Minigame
{
    [SerializeField] private Button _finishButton;
    [SerializeField] private KahootInitialQuestionView _kahootInitialQuestionView;
    [SerializeField] private KahootQuestionView _kahootQuestionView;
    [SerializeField] private KahootResultsView _kahootResultsView;

    private List<string> _answers = new();

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessageToServer("kahoot");

        string[] minigameInputData = minigameData.Input.Split(',');
        var question = minigameInputData[0];
        var picture = Resources.Load<Sprite>(minigameInputData[1]);

        int startingAnswersIndex = 2;
        for (int i = startingAnswersIndex; i < minigameInputData.Length; i++)
        {
            _answers.Add(minigameInputData[i]);
        }

        _kahootInitialQuestionView.Initialize(question, OpenQuestionView);
        _kahootQuestionView.Initialize(question, picture, _answers, OpenResultView);

        _finishButton.onClick.AddListener(FinishGame);
    }

    private void OpenQuestionView()
    {
        _kahootInitialQuestionView.gameObject.SetActive(false);
        _kahootQuestionView.gameObject.SetActive(true);
    }

    private void OpenResultView()
    {
        _kahootQuestionView.gameObject.SetActive(false);
        _kahootResultsView.gameObject.SetActive(true);
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
