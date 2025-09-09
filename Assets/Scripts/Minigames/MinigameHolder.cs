using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum MinigameType
{
    None,
    Question,
    Hangman,
    Geoguessr,
    Music,
    ZoomIn,
    RandomizedWord
}

public class MinigameHolder : MonoBehaviour
{
    [SerializeField] private MinigameType _minigameType;
    [SerializeField] private MinigameSpawner _minigameSpawner;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _text;

    private UnityAction _callback;

    private void Start()
    {
        _callback = () => _minigameSpawner.OpenMinigame(_minigameType, transform.GetSiblingIndex());

        if (_minigameType != MinigameType.None)
        {
            _button.onClick.AddListener(_callback);
            _button.interactable = true;
        }
        else
        {
            _button.interactable = false;
        }
    }

    private void Show()
    {
        _button.enabled = true;
        _text.enabled = true;
    }

    private void Hide()
    {
        _button.enabled = false;
        _text.enabled = false;
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(_callback);
    }
}
