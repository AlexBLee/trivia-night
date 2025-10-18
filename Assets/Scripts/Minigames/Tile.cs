using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    [SerializeField] private TMP_Text _numberText;
    [SerializeField] private Button _button;

    private Action<Tile> _onClickCallback;
    public int Number { get; private set; }

    public void Init(int number, Action<Tile> onClick)
    {
        Number = number;
        _numberText.text = number.ToString();
        _onClickCallback = onClick;
        _button.onClick.AddListener(() => _onClickCallback?.Invoke(this));
    }

    public void HideNumber()
    {
        _numberText.gameObject.SetActive(false);
    }
}