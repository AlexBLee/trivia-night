using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _defaultTimerDurationInSeconds;

    private float _timeRemaining;
    private float _minuteDivisor = 60f;
    private bool _subscribersNotified = false;

    public Action OnTimerEnd;

    private void OnEnable()
    {
        _timeRemaining = _defaultTimerDurationInSeconds;
        _subscribersNotified = false;
    }

    private void Update()
    {
        if (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(_timeRemaining / _minuteDivisor);
            int seconds = Mathf.FloorToInt(_timeRemaining % _minuteDivisor);
            _timerText.text = $"{minutes:0}:{seconds:00}";
        }
        else
        {
            if (_subscribersNotified)
            {
                return;
            }

            OnTimerEnd?.Invoke();
            _subscribersNotified = true;
        }
    }
}
