using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KahootQuestionView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private Image _questionImage;
    [SerializeField] private List<TextMeshProUGUI> _answerTexts;

    [SerializeField] private RectTransform _timerLine;
    [SerializeField] private float _duration;

    private Action _onTimerComplete;

    public void Initialize(string question, Sprite questionImage, List<string> answers, Action onComplete = null)
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

        _onTimerComplete = onComplete;
    }

    private void OnEnable()
    {
        _timerLine.DOSizeDelta(new Vector2(0, _timerLine.sizeDelta.y), _duration).OnComplete(() =>
        {
            _onTimerComplete?.Invoke();
        });
    }
}