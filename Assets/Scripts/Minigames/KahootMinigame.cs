using System;
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
    private List<KahootAnswer> _teamAnswers = new();

    private float _questionTime = 10f;
    private int _scoreAmount = 100;
    private DateTime _startTime;
    private int _answerIndex;

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

        _answerIndex = int.Parse(minigameData.Answer);

        _kahootInitialQuestionView.Initialize(question, OpenQuestionView);
        _kahootQuestionView.Initialize(question, picture, _answers, _answerIndex);

        _kahootQuestionView.UpdateAnswers(_teamAnswers);

        _finishButton.onClick.AddListener(FinishGame);
    }

    private void OpenQuestionView()
    {
        _kahootInitialQuestionView.gameObject.SetActive(false);
        _kahootQuestionView.gameObject.SetActive(true);
    }

    protected override void ReceiveMessage(IWebSocketConnection socket, string message)
    {
        base.ReceiveMessage(socket, message);

        var team = _teamManager.GetTeam(socket);
        var kahootAnswer = new KahootAnswer()
        {
            Team = team,
            AnswerIndex = int.Parse(message),
            TimeTaken = DateTime.Now,
        };

        kahootAnswer.Score = CalculateScore(kahootAnswer);

        _teamAnswers.Add(kahootAnswer);
        _kahootQuestionView.UpdateAnswers(_teamAnswers);
    }

    private int CalculateScore(KahootAnswer kahootAnswer)
    {
        if (kahootAnswer.AnswerIndex != _answerIndex)
        {
            return 0;
        }

        var score = 0;
        var timeDiff = (kahootAnswer.TimeTaken - _startTime).TotalSeconds;

        float bonus = Mathf.Clamp01((float) (1f - timeDiff / _questionTime));
        score = Mathf.RoundToInt(_scoreAmount + _scoreAmount * bonus);

        return score;
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        _finishButton.onClick.RemoveListener(FinishGame);
    }
}
