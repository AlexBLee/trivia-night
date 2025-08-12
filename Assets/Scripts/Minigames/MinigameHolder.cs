using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameHolder : MonoBehaviour
{
    [SerializeField] private Minigame _minigame;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        if (_minigame != null)
        {
            _button.onClick.AddListener(_minigame.Initialize);
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
}
