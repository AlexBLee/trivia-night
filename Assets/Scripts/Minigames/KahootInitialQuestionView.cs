using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class KahootInitialQuestionView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private RectTransform _timerLine;
    [SerializeField] private float _duration;

    public void Initialize(string question, Action onComplete = null)
    {
        _questionText.text = question;
        _timerLine.DOSizeDelta(new Vector2(0, _timerLine.sizeDelta.y), _duration).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
