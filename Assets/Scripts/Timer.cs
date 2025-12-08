using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _defaultTimerDurationInSeconds;

    private float _timeRemaining;
    private float _minuteDivisor = 60f;
    private bool _subscribersNotified = false;
    private float _animationDuration = 0.5f;

    private bool _playingTimerSound = false;
    private float _playSoundTime = 10f;

    public Action OnTimerEnd;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, _animationDuration);

        _timeRemaining = _defaultTimerDurationInSeconds;
        _playingTimerSound = false;
        _subscribersNotified = false;
    }

    private void Update()
    {
        if (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;

            if (!_playingTimerSound && _timeRemaining < _playSoundTime)
            {
                AudioManager.Instance.PlaySfx("GeoguessrTimer");
                _playingTimerSound = true;
            }

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
