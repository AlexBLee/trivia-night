using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KahootQuestionView : MonoBehaviour
{
    [SerializeField] private KahootScoreView _kahootScoreView;
    [SerializeField] private KahootResultsView _kahootResultsView;

    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private Image _questionImage;
    [SerializeField] private List<TextMeshProUGUI> _answerTexts;
    [SerializeField] private List<Button> _answerButtons;

    [SerializeField] private RectTransform _timerLine;
    [SerializeField] private float _duration;

    private int _correctAnswerIndex;
    private List<KahootAnswer> _teamAnswers = new();

    public void Initialize(string question, Sprite questionImage, List<string> answers, int correctAnswerIndex)
    {
        _questionText.text = question;
        _questionImage.sprite = questionImage;

        if (answers.Count != _answerTexts.Count)
        {
            Debug.LogWarning("Wrong number of answers");
            return;
        }

        for (int i = 0; i < answers.Count; i++)
        {
            _answerTexts[i].text = answers[i];
        }

        _correctAnswerIndex = correctAnswerIndex;
    }

    public void UpdateAnswers(List<KahootAnswer> teamAnswers)
    {
        _teamAnswers = teamAnswers;
    }

    private void OpenScores()
    {
        _questionImage.gameObject.SetActive(false);
        _kahootScoreView.gameObject.SetActive(true);

        for (int i = 0; i < _answerButtons.Count; i++)
        {
            if (i != _correctAnswerIndex)
            {
                _answerButtons[i].interactable = false;
            }
        }

        _kahootScoreView.SubmitScores(_teamAnswers);
    }

    private void OnEnable()
    {
        _timerLine.DOSizeDelta(new Vector2(0, _timerLine.sizeDelta.y), _duration).OnComplete(OpenScores);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            gameObject.SetActive(false);
            _kahootResultsView.gameObject.SetActive(true);
            _kahootResultsView.CalculateScore(_teamAnswers);
        }
    }
}