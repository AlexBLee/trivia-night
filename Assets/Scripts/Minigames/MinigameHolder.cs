using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum MinigameType
{
    None,
    Question,
    Hangman,
    Geoguessr,
    Music,
    ZoomIn,
    RandomizedWord,
    ChimpTest,
    CoinFlip,
    BombOrBonus,
    Wheel,
    CardFlip,
    PictureGuessing,
    Kahoot
}

public class MinigameHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private MinigameType _minigameType;
    [SerializeField] private MinigameSpawner _minigameSpawner;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _text;

    private bool _isHovered = false;
    private UnityAction _callback;

    private void Start()
    {
        _callback = () =>
        {
            _minigameSpawner.OpenMinigame(transform.GetSiblingIndex());
            _button.interactable = false;
            _text.enabled = false;
            _isHovered = false;
        };

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

    private void Update()
    {
        if (_isHovered && Input.GetKeyDown(KeyCode.F))
        {
            _button.interactable = true;
            _text.enabled = true;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovered = false;
    }
}
